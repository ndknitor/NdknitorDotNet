using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.Authentication;
using Ndknitor.Services.Web;

BenchmarkRunner.Run<KeyBasedCookieDataFormatBenchmark>();


public class KeyBasedCookieDataFormatBenchmark
{
    private readonly KeyBasedCookieDataFormat cookieDataFormat;
    private readonly AuthenticationTicket authenticationTicket;

    public KeyBasedCookieDataFormatBenchmark()
    {
        string authenticationKey = "YourAuthenticationKey";
        cookieDataFormat = new KeyBasedCookieDataFormat(authenticationKey);

        var claimsIdentity = new ClaimsIdentity(new[] { new Claim("claim1", "value1"), new Claim("claim2", "value2") });
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        authenticationTicket = new AuthenticationTicket(claimsPrincipal, "TestScheme");
    }

    [Benchmark]
    public string Protect()
    {
        return cookieDataFormat.Protect(authenticationTicket, "purpose");
    }

    [Benchmark]
    public AuthenticationTicket Unprotect()
    {
        string protectedText = cookieDataFormat.Protect(authenticationTicket, "purpose");
        return cookieDataFormat.Unprotect(protectedText, "purpose");
    }
}
