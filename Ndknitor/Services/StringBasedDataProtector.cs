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
        // Implement your custom encryption logic using the provided key
        // Note: This is a basic example and is not secure for production use
        return userData;
    }

    public byte[] Unprotect(byte[] protectedData)
    {
        // Implement your custom decryption logic using the provided key
        // Note: This is a basic example and is not secure for production use
        return protectedData;
    }
}
