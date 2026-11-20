using System;
using System.Security.Cryptography;

namespace KooliProjekt.Application.Authentication
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        public static string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
            var combined = new byte[SaltSize + HashSize];
            Buffer.BlockCopy(salt, 0, combined, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, combined, SaltSize, HashSize);
            return Convert.ToBase64String(combined);
        }

        public static bool Verify(string password, string storedPassword)
        {
            if (string.IsNullOrEmpty(storedPassword)) return false;
            try
            {
                var combined = Convert.FromBase64String(storedPassword);
                if (combined.Length != SaltSize + HashSize) return FixedEquals(password, storedPassword);
                var salt = new byte[SaltSize];
                var expected = new byte[HashSize];
                Buffer.BlockCopy(combined, 0, salt, 0, SaltSize);
                Buffer.BlockCopy(combined, SaltSize, expected, 0, HashSize);
                var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
                return CryptographicOperations.FixedTimeEquals(expected, actual);
            }
            catch (FormatException) { return FixedEquals(password, storedPassword); }
        }

        private static bool FixedEquals(string value, string expected)
        {
            var a = System.Text.Encoding.UTF8.GetBytes(value ?? string.Empty);
            var b = System.Text.Encoding.UTF8.GetBytes(expected ?? string.Empty);
            return a.Length == b.Length && CryptographicOperations.FixedTimeEquals(a, b);
        }
    }
}
