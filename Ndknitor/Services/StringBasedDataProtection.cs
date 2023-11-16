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
    private readonly Aes aes;
    private readonly ICryptoTransform encryptor;
    private readonly ICryptoTransform decryptor;

    public StringBasedDataProtector(string key)
    {
        this.key = key;
        aes = Aes.Create();
        aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        aes.Mode = CipherMode.CFB;
        aes.Padding = PaddingMode.PKCS7;
        encryptor = aes.CreateEncryptor();
        decryptor = aes.CreateDecryptor();
    }

    public IDataProtector CreateProtector(string purpose)
    {
        return new StringBasedDataProtector(key); // You can implement purpose-based protection if needed
    }

    public byte[] Protect(byte[] userData)
    {
        return encryptor.TransformFinalBlock(userData, 0, userData.Length);
        // using (var aes = Aes.Create())
        // {
        //     aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        //     aes.Mode = CipherMode.CFB;
        //     aes.Padding = PaddingMode.PKCS7;

        //     using (var encryptor = aes.CreateEncryptor())
        //     {
        //         return encryptor.TransformFinalBlock(userData, 0, userData.Length);
        //     }
        // }
    }

    public byte[] Unprotect(byte[] protectedData)
    {
        return decryptor.TransformFinalBlock(protectedData, 0, protectedData.Length);
        // using (var aes = Aes.Create())
        // {
        //     aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        //     aes.Mode = CipherMode.CFB;
        //     aes.Padding = PaddingMode.PKCS7;

        //     using (var decryptor = aes.CreateDecryptor())
        //     {
        //         return decryptor.TransformFinalBlock(protectedData, 0, protectedData.Length);
        //     }
        // }
    }
}
