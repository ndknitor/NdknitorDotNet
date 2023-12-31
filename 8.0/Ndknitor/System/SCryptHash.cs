using System.Collections;
using System.Security.Cryptography;

namespace Ndknitor.System;
public class SCryptHash
{
    public int SaltSize { get; set; } = 16;
    public int HashSize { get; set; } = 48;
    public int BlockSize { get; set; } = 8;
    public int Cost { get; set; } = 16384;
    public int Parallel { get; set; } = 1;
    public byte[] Hash(byte[] password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Scrypt(password, salt, Cost, BlockSize, Parallel, HashSize);
        var combinedBytes = new byte[salt.Length + hash.Length];
        Array.Copy(salt, 0, combinedBytes, 0, salt.Length);
        Array.Copy(hash, 0, combinedBytes, salt.Length, hash.Length);
        return combinedBytes;
    }
    public byte[] RawHash(byte[] password, byte[] salt, int cost, int blockSize, int parallel, int size)
    {
        return Scrypt(password, salt, cost, blockSize, parallel, size);
    }
    public bool Verify(byte[] password, byte[] hashedPassword)
    {
        var salt = new byte[SaltSize];
        var hash = new byte[HashSize];
        Array.Copy(hashedPassword, 0, salt, 0, salt.Length);
        Array.Copy(hashedPassword, salt.Length, hash, 0, hash.Length);
        var enteredHash = Scrypt(password, salt, Cost, BlockSize, Parallel, HashSize);
        //SCrypt.ComputeDerivedKey(password, salt, Cost, BlockSize, Parallel, null, HashSize);
        return StructuralComparisons.StructuralEqualityComparer.Equals(hash, enteredHash);
    }
    private byte[] Scrypt(ReadOnlySpan<byte> password, ReadOnlySpan<byte> salt, int N, int r, int p, int dkLen)
    {
        if (N < 2 || (N & (N - 1)) != 0) throw new ArgumentException("N must be a power of 2 greater than 1", nameof(N));

        if (N > int.MaxValue / 128 / r) throw new ArgumentException("Parameter N is too large", nameof(N));
        if (r > int.MaxValue / 128 / p) throw new ArgumentException("Parameter r is too large", nameof(r));

        byte[] DK;

        byte[] B;
        byte[] XY = new byte[256 * r];
        byte[] V = new byte[128 * r * N];
        int i;
        using (var mac = new HMACSHA256(password.ToArray()))
        {
            B = PBKDF2_SHA256(mac, salt, 1, p * 128 * r);

            for (i = 0; i < p; i++)
            {
                Smix(B, i * 128 * r, r, N, V, XY);
            }

            DK = PBKDF2_SHA256(mac, B, 1, dkLen);
        }

        return DK;
    }

    private byte[] PBKDF2_SHA256(HMACSHA256 mac, ReadOnlySpan<byte> salt, long iterationCount, int derivedKeyLength)
    {
        if (derivedKeyLength > (Math.Pow(2, 32) - 1) * 32)
        {
            throw new ArgumentException("Requested key length too long");
        }

        Span<byte> U = stackalloc byte[32];
        Span<byte> T = stackalloc byte[32];
        var saltLength = salt.Length;
        var saltBuffer = new byte[saltLength + 4];
        var derivedKey = new byte[derivedKeyLength];

        var blockCount = (int)Math.Ceiling((double)derivedKeyLength / 32);
        var r = derivedKeyLength - (blockCount - 1) * 32;

        salt.CopyTo(saltBuffer);


        for (int i = 1; i <= blockCount; i++)
        {
            var blockLength = (i == blockCount ? r : 32);
            saltBuffer[saltLength + 0] = (byte)(i >> 24);
            saltBuffer[saltLength + 1] = (byte)(i >> 16);
            saltBuffer[saltLength + 2] = (byte)(i >> 8);
            saltBuffer[saltLength + 3] = (byte)(i);

            mac.Initialize();
            mac.TryComputeHash(saltBuffer, U, out var len);
            U.CopyTo(T);

            for (long j = 1; j < iterationCount; j++)
            {
                mac.TryComputeHash(U, U, out len);
                for (int k = 0; k < 32; k++)
                {
                    T[k] ^= U[k];
                }
            }

            T.Slice(0, blockLength).CopyTo(new Span<byte>(derivedKey, (i - 1) * 32, blockLength));
        }

        return derivedKey;
    }

    private void Smix(Span<byte> B, int Bi, int r, int N, Span<byte> V, Span<byte> XY)
    {
        int Xi = 0;
        int Yi = 128 * r;
        int i;

        B.Slice(Bi, Yi).CopyTo(XY.Slice(Xi, Yi));

        for (i = 0; i < N; i++)
        {
            XY.Slice(Xi, Yi).CopyTo(V.Slice(i * Yi, Yi));
            BlockmixSalsa8(XY, Xi, Yi, r);
        }

        for (i = 0; i < N; i++)
        {
            int j = Integerify(XY.Slice(Xi + (2 * r - 1) * 64, 4)) & (N - 1);
            Blockxor(V.Slice(j * Yi, Yi), XY.Slice(Xi, Yi));
            BlockmixSalsa8(XY, Xi, Yi, r);
        }

        XY.Slice(Xi, Yi).CopyTo(B.Slice(Bi, Yi));
    }

    private void BlockmixSalsa8(Span<byte> BY, int Bi, int Yi, int r)
    {
        Span<byte> X = stackalloc byte[64];
        int i;

        BY.Slice(Bi + (2 * r - 1) * 64, 64).CopyTo(X);

        for (i = 0; i < 2 * r; i++)
        {
            Blockxor(BY.Slice(i * 64, 64), X);
            Salsa20_8(X);
            X.CopyTo(BY.Slice(Yi + (i * 64), 64));
        }

        for (i = 0; i < r; i++)
        {
            BY.Slice(Yi + (i * 2) * 64, 64).CopyTo(BY.Slice(Bi + (i * 64), 64));
        }

        for (i = 0; i < r; i++)
        {
            BY.Slice(Yi + (i * 2 + 1) * 64, 64).CopyTo(BY.Slice(Bi + (i + r) * 64, 64));
        }
    }

    private uint R(uint a, int b)
    {
        return (a << b) | (a >> (32 - b));
    }

    private unsafe void Salsa20_8(Span<byte> B)
    {
        fixed (byte* bptr = B)
        {
            uint* uptr = (uint*)bptr;
            uint* x = stackalloc uint[16];
            for (int i = 0; i < 16; i++)
            {
                x[i] = uptr[i];
            }
            for (int i = 0; i < 8; i += 2)
            {
                x[4] ^= R(x[0] + x[12], 7); x[8] ^= R(x[4] + x[0], 9);
                x[12] ^= R(x[8] + x[4], 13); x[0] ^= R(x[12] + x[8], 18);

                x[9] ^= R(x[5] + x[1], 7); x[13] ^= R(x[9] + x[5], 9);
                x[1] ^= R(x[13] + x[9], 13); x[5] ^= R(x[1] + x[13], 18);

                x[14] ^= R(x[10] + x[6], 7); x[2] ^= R(x[14] + x[10], 9);
                x[6] ^= R(x[2] + x[14], 13); x[10] ^= R(x[6] + x[2], 18);

                x[3] ^= R(x[15] + x[11], 7); x[7] ^= R(x[3] + x[15], 9);
                x[11] ^= R(x[7] + x[3], 13); x[15] ^= R(x[11] + x[7], 18);

                /* Operate on rows. */
                x[1] ^= R(x[0] + x[3], 7); x[2] ^= R(x[1] + x[0], 9);
                x[3] ^= R(x[2] + x[1], 13); x[0] ^= R(x[3] + x[2], 18);

                x[6] ^= R(x[5] + x[4], 7); x[7] ^= R(x[6] + x[5], 9);
                x[4] ^= R(x[7] + x[6], 13); x[5] ^= R(x[4] + x[7], 18);

                x[11] ^= R(x[10] + x[9], 7); x[8] ^= R(x[11] + x[10], 9);
                x[9] ^= R(x[8] + x[11], 13); x[10] ^= R(x[9] + x[8], 18);

                x[12] ^= R(x[15] + x[14], 7); x[13] ^= R(x[12] + x[15], 9);
                x[14] ^= R(x[13] + x[12], 13); x[15] ^= R(x[14] + x[13], 18);
            }

            for (int i = 0; i < 16; i++)
                uptr[i] += x[i];
        }
    }

    private void Blockxor(ReadOnlySpan<byte> S, Span<byte> D)
    {
        for (int i = 0; i < S.Length; i++)
        {
            D[i] ^= S[i];
        }
    }

    private unsafe int Integerify(ReadOnlySpan<byte> B)
    {
        fixed (byte* bptr = B)
        {
            int* iptr = (int*)bptr;
            return *iptr;
        }
    }
}