using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MyFace.Helpers
{
    public class PasswordHelper
    {
        public string Password { get; set; }
        public byte[] Salt { get; set; }
        public string HashedPassword { get; set; }

        public PasswordHelper(string password)
        {
            Password = password;
            Salt = getSalt();
            HashedPassword = getHashedPassword(Password, Salt);
        }

        private byte[] getSalt()
        {
            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");
            return salt;
        }

        public string getHashedPassword(string password, byte[] salt)
        {
            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            Console.WriteLine($"Hashed: {hashed}");
            return hashed;
        }

    }
}