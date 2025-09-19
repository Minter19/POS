using System.Security.Cryptography;

namespace Shared.Helper
{
    public static class PasswordHelper
    {
        // Recommended values are at least 100,000 for modern systems.
        private const int Iterations = 100000;

        // The size of the salt in bytes.
        private const int SaltSize = 16;

        // The size of the derived hash in bytes.
        private const int HashSize = 32;

        public static int Iterations1 => Iterations;

        public static int SaltSize1 => SaltSize;

        public static int HashSize1 => HashSize;

        public static string HashPassword(string password)
        {
            // Generate a random salt for each new password hash.
            byte[] salt = new byte[SaltSize1];
            RandomNumberGenerator.Fill(salt);

            // Use SHA256 as the hash algorithm for PBKDF2.
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations1, HashAlgorithmName.SHA256);

            // Get the hash from the instance.
            byte[] hash = pbkdf2.GetBytes(HashSize1);

            // Combine the salt and hash.
            byte[] hashBytes = new byte[SaltSize1 + HashSize1];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize1);
            Array.Copy(hash, 0, hashBytes, SaltSize1, HashSize1);

            // Convert the combined bytes to a string.
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Verifies a plain-text password against a stored hashed password.
        /// </summary>
        /// <param name="password">The plain-text password to verify.</param>
        /// <param name="hashedPassword">The stored hashed password from the database.</param>
        /// <returns>True if the passwords match, false otherwise.</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Convert the stored hashed password string back to a byte array.
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // Extract the salt from the byte array.
            byte[] salt = new byte[SaltSize1];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize1);

            // Use SHA256 as the hash algorithm for PBKDF2.
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations1, HashAlgorithmName.SHA256);
            byte[] testHash = pbkdf2.GetBytes(HashSize1);

            // Compare the re-created hash with the stored hash.
            for (int i = 0; i < HashSize1; i++)
            {
                if (hashBytes[i + SaltSize1] != testHash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
