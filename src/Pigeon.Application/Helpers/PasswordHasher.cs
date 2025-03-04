using System.Security.Cryptography;

namespace Pigeon.Application.Helpers;

public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 10000;

    public static (string HashedPassword, string Salt) HashPassword(string password, byte[]? salt = null)
    {
        if (salt is null) salt = GenerateSalt(SaltSize);

        using var rfc2898 = new Rfc2898DeriveBytes(password, salt, Iterations,
            HashAlgorithmName.SHA256);
        string hashedPassword = Convert.ToBase64String(rfc2898.GetBytes(HashSize));

        return (hashedPassword, Convert.ToBase64String(salt));
    }

    private static byte[] GenerateSalt(int size)
    {
        using var rng = RandomNumberGenerator.Create();
        byte[] salt = new byte[size];
        rng.GetBytes(salt);
        return salt;
    }
}