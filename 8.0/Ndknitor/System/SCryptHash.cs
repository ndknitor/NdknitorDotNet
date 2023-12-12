using System.Security.Cryptography;
using CryptSharp.Utility;
public class SCryptHash
{
    public int SaltSize { get; set; } = 16;
    public int Cost { get; set; } = 262144;
    public int BlockSize { get; set; } = 8;
    public int Parallel { get; set; } = 1;
    public int Size { get; set; } = 64;
    public byte[] Hash(byte[] password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] combined = new byte[SaltSize + Size];
        byte[] hash = SCrypt.ComputeDerivedKey(password, salt, Cost, BlockSize, Parallel, null, Size);
        Array.Copy(salt, 0, combined, 0, salt.Length);
        Array.Copy(hash, 0, combined, salt.Length, hash.Length);
        return combined;
    }
}