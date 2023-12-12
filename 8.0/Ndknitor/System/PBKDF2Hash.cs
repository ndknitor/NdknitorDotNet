using System.Collections;
using System.Security.Cryptography;
namespace Ndknitor.System;
public class PBKDF2Hash
{
    public int Iteration { get; set; } = 200000;
    public int SaltSize { get; set; } = 16;
    public int HashSize { get; set; } = 48;
    public HashAlgorithmName AlgorithmName { get; set; } = HashAlgorithmName.SHA512;
    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iteration, AlgorithmName))
        {
            var hash = pbkdf2.GetBytes(HashSize);
            var combinedBytes = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, combinedBytes, 0, salt.Length);
            Array.Copy(hash, 0, combinedBytes, salt.Length, hash.Length);
            return Convert.ToBase64String(combinedBytes);
        }
    }
    public byte[] Hash(byte[] password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iteration, AlgorithmName))
        {
            var hash = pbkdf2.GetBytes(HashSize);
            var combinedBytes = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, combinedBytes, 0, salt.Length);
            Array.Copy(hash, 0, combinedBytes, salt.Length, hash.Length);
            return combinedBytes;
        }
    }
    public bool Verify(byte[] password, byte[] hashedPassword)
    {
        var salt = new byte[SaltSize];
        var hash = new byte[HashSize];
        Array.Copy(hashedPassword, 0, salt, 0, salt.Length);
        Array.Copy(hashedPassword, salt.Length, hash, 0, hash.Length);
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iteration, AlgorithmName))
        {
            var enteredHash = pbkdf2.GetBytes(HashSize);
            return StructuralComparisons.StructuralEqualityComparer.Equals(hash, enteredHash);
        }
    }
    public bool Verify(string password, string hashedPassword)
    {
        var combinedBytes = Convert.FromBase64String(hashedPassword);
        var salt = new byte[SaltSize];
        var hash = new byte[HashSize];
        Array.Copy(combinedBytes, 0, salt, 0, salt.Length);
        Array.Copy(combinedBytes, salt.Length, hash, 0, hash.Length);
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iteration, AlgorithmName))
        {
            var enteredHash = pbkdf2.GetBytes(HashSize);
            return StructuralComparisons.StructuralEqualityComparer.Equals(hash, enteredHash);
        }
    }
}