using System.Security.Claims;
namespace Ndknitor.System;
public static class ClaimsPrincipalExtension
{
    /// <summary>
    /// Extract a specific user identifier from the claims associated with a ClaimsPrincipal object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="principal"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static T NameIdentifier<T>(this ClaimsPrincipal principal)
    where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        return (T)Convert.ChangeType(principal.FindFirstValue(ClaimTypes.NameIdentifier), typeof(T));
    }
    public static T GetValue<T>(this ClaimsPrincipal principal, string claimType)
    {
        return (T)Convert.ChangeType(principal.FindFirstValue(claimType), typeof(T));
    }
}