using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
namespace Ndknitor.Services.Web;
using Base58Check;
/// <summary>
/// This provide cookie authentication with the ability to use a customizable encryption key.
/// </summary>
public class KeyBasedCookieDataFormat : ISecureDataFormat<AuthenticationTicket>
{
    private readonly byte[] key;
    public KeyBasedCookieDataFormat(string authenticationKey)
    {
        using (var sha256 = SHA256.Create())
        {
            key = sha256.ComputeHash(Encoding.UTF8.GetBytes(authenticationKey));
        }
    }

    public string Protect(AuthenticationTicket data)
    {
        throw new NotImplementedException("Protect without purpose is not supported.");
    }

    public string Protect(AuthenticationTicket data, string purpose)
    {
        var ticketSerializer = new TicketSerializer();
        var ticketBytes = ticketSerializer.Serialize(data);
        string protectedText = Base58CheckEncoding.Encode(EncryptAes(ticketBytes));
        return protectedText;

    }

    public AuthenticationTicket Unprotect(string protectedText)
    {
        throw new NotImplementedException("Unprotect without purpose is not supported.");
    }

    public AuthenticationTicket Unprotect(string protectedText, string purpose)
    {
        try
        {
            var encryptedBytes = Base58CheckEncoding.Decode(protectedText);
            var decryptedBytes = DecryptAes(encryptedBytes);

            var ticketSerializer = new TicketSerializer();
            var ticket = ticketSerializer.Deserialize(decryptedBytes);
            return ticket;
        }
        catch
        {
            return null;
        }

    }
    byte[] EncryptAes(byte[] plain)
    {
        byte[] result = null;
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Padding = PaddingMode.ISO10126;
            aesAlg.GenerateIV();
            aesAlg.Key = key;
            using (ICryptoTransform encryptor = aesAlg.CreateEncryptor())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(plain, 0, plain.Length);
                        cs.FlushFinalBlock();
                    }
                    result = ms.ToArray();
                }
            }
        }
        return result;
    }

    byte[] DecryptAes(byte[] clipher)
    {
        byte[] result = null;
        using (Aes aesAlg = Aes.Create())
        {
            byte[] iv = new byte[16];
            Buffer.BlockCopy(clipher, 0, iv, 0, iv.Length);
            aesAlg.Padding = PaddingMode.ISO10126;
            aesAlg.Key = key;
            aesAlg.IV = iv;
            ICryptoTransform decryptor = aesAlg.CreateDecryptor();
            result = decryptor.TransformFinalBlock(clipher, iv.Length, clipher.Length - iv.Length);
        }
        return result;
    }

}
