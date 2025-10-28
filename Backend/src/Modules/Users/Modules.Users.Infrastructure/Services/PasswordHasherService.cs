using System;
using System.Security.Cryptography;
using Modules.Users.Application.UseCases.Contracts.Services;

namespace Modules.Users.Infrastructure.Services;

internal sealed class PasswordHasherService : IPasswordHasherService
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 500_000;

    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);
        return $"{Iterations}.{Algorithm.Name}.{Convert.ToHexString(hash)}.{Convert.ToHexString(salt)}";
    }

    public bool Verify(string password, string passwordHash)
    {
        var parts = passwordHash.Split('.');
        if (parts.Length != 4)
            return false;

        int iterations = int.Parse(parts[0]);
        var algorithm = new HashAlgorithmName(parts[1]);
        byte[] hash = Convert.FromHexString(parts[2]);
        byte[] salt = Convert.FromHexString(parts[3]);

        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, algorithm, hash.Length);

        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }
}