using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public class AspNetMembershipPasswordHasher : IPasswordHasher<AspNetMembershipUser>
    {
        public string HashPassword(AspNetMembershipUser user, string password)
        {
            string passwordHash = null;
            string passwordSalt = null;

            this.HashPassword(password, out passwordHash, ref passwordSalt);

            user.PasswordFormat = 1;
            user.PasswordSalt = passwordSalt;
            return passwordHash;
        }

        public PasswordVerificationResult VerifyHashedPassword(AspNetMembershipUser user, string hashedPassword, string providedPassword)
        {
            // Throw an error if any of our passwords are null
            if (hashedPassword == null)
            {
                throw new ArgumentNullException("hashedPassword");
            }

            if (providedPassword == null)
            {
                throw new ArgumentNullException("providedPassword");
            }

            string providedPasswordHash = null;

            if (user.PasswordFormat == 0)
            {
                providedPasswordHash = providedPassword;
            }
            else if (user.PasswordFormat == 1)
            {

                string providedPasswordSalt = user.PasswordSalt;

                this.HashPassword(providedPassword, out providedPasswordHash, ref providedPasswordSalt);
            }
            else
            {
                throw new NotSupportedException("Encrypted passwords are not supported.");
            }

            if (providedPasswordHash == hashedPassword)
            {
                return PasswordVerificationResult.Success;
            }
            else
            {
                return PasswordVerificationResult.Failed;
            }
        }

        private void HashPassword(string password, out string passwordHash, ref string passwordSalt)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            byte[] saltBytes = null;

            if (!string.IsNullOrEmpty(passwordSalt))
            {
                saltBytes = Convert.FromBase64String(passwordSalt);
            }
            else
            {
                saltBytes = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(saltBytes);
                }
            }

            byte[] totalBytes = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, totalBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, totalBytes, saltBytes.Length, passwordBytes.Length);

            using (SHA1 hashAlgorithm = SHA1.Create())
            {
                passwordHash = Convert.ToBase64String(hashAlgorithm.ComputeHash(totalBytes));
            }

            passwordSalt = Convert.ToBase64String(saltBytes);
        }
    }
}
