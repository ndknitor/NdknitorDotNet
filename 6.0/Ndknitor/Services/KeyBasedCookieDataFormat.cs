using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
namespace Ndknitor.Services.Web;
public class KeyBasedCookieDataFormat : ISecureDataFormat<AuthenticationTicket>
{
    private readonly byte[] encryptionKey;
    private readonly Aes aesAlg;
    private ICryptoTransform encryptor;
    private ICryptoTransform decryptor;

    public KeyBasedCookieDataFormat(string authenticationKey)
    {
        using (var sha256 = SHA256.Create())
        {
            encryptionKey = sha256.ComputeHash(Encoding.UTF8.GetBytes(authenticationKey));
            aesAlg = Aes.Create();
            aesAlg.Key = encryptionKey;
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.PKCS7;
            encryptor = aesAlg.CreateEncryptor();
            decryptor = aesAlg.CreateDecryptor();
        }
    }

    public string Protect(AuthenticationTicket data)
    {
        throw new NotImplementedException("Protect without purpose is not supported.");
    }

    public string Protect(AuthenticationTicket data, string purpose)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        var ticketSerializer = new TicketSerializer();
        var ticketBytes = ticketSerializer.Serialize(data);
        string protectedText = Convert.ToBase64String(encryptor.TransformFinalBlock(ticketBytes, 0, ticketBytes.Length));
        return protectedText;

    }

    public AuthenticationTicket Unprotect(string protectedText)
    {
        throw new NotImplementedException("Unprotect without purpose is not supported.");
    }

    public AuthenticationTicket Unprotect(string protectedText, string purpose)
    {
        if (string.IsNullOrEmpty(protectedText))
        {
            throw new ArgumentNullException(nameof(protectedText));
        }
        try
        {
            var encryptedBytes = Convert.FromBase64String(protectedText);
            var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            var ticketSerializer = new TicketSerializer();
            var ticket = ticketSerializer.Deserialize(decryptedBytes);
            return ticket;
        }
        catch
        {
            return null;
        }

    }
}
