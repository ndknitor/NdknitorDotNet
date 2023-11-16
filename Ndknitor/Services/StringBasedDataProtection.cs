using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.DataProtection;

public static class DataProtectionServiceCollectionExtensions
{
    public static void AddStringBasedDataProtection(this IServiceCollection services, string key)
    {
        services.AddSingleton<IDataProtectionProvider>(new StringBasedDataProtectionProvider(key));
    }
}

public class StringBasedDataProtectionProvider : IDataProtectionProvider
{
    private readonly string customKey;

    public StringBasedDataProtectionProvider(string customKey)
    {
        this.customKey = customKey;
    }

    public IDataProtector CreateProtector(string purpose)
    {
        return new StringBasedDataProtector(customKey);
    }
}

public class StringBasedDataProtector : IDataProtector
{
    private readonly string key;

    public StringBasedDataProtector(string key)
    {
        this.key = key;
    }

    public IDataProtector CreateProtector(string purpose)
    {
        return new StringBasedDataProtector(key); // You can implement purpose-based protection if needed
    }

    public byte[] Protect(byte[] userData)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(key));
            aes.Mode = CipherMode.CFB;
            aes.Padding = PaddingMode.PKCS7;

            using (var encryptor = aes.CreateEncryptor())
            {
                return encryptor.TransformFinalBlock(userData, 0, userData.Length);
            }
        }
    }

    public byte[] Unprotect(byte[] protectedData)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(key));
            aes.Mode = CipherMode.CFB;
            aes.Padding = PaddingMode.PKCS7;

            using (var decryptor = aes.CreateDecryptor())
            {
                return decryptor.TransformFinalBlock(protectedData, 0, protectedData.Length);
            }
        }
    }
}
