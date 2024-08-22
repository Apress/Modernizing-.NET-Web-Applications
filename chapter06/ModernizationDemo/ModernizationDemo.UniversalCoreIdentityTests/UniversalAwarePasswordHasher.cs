using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace ModernizationDemo.UniversalCoreIdentityTests
{
    public class UniversalAwarePasswordHasher : PasswordHasher<AppUser>
    {
        public override PasswordVerificationResult VerifyHashedPassword(AppUser user, string hashedPassword, string providedPassword)
        {
            // is the password in the old format?
            var passwordData = Convert.FromBase64String(hashedPassword);
            if (passwordData[0] == 0xff)
            {
                // extract the legacy salt and double hashed password
                ExtractSaltAndDoubleHashedPassword(passwordData, out var legacySalt, out var encodedPassword);

                // calculate original PasswordHash using the legacy algorithm
                // and use it as it was the user provided password
                providedPassword = HashPasswordUsingLegacyAlgorithm(providedPassword, legacySalt);

                // use the double-hashed password as the value for verification
                hashedPassword = Convert.ToBase64String(encodedPassword);
            }

            // use the default algorithm the password
            var result = base.VerifyHashedPassword(user, hashedPassword, providedPassword);

            // if the old format was used and credentials were valid, rehash
            if (result == PasswordVerificationResult.Success && passwordData[0] == 0xFF)
            {
                return PasswordVerificationResult.SuccessRehashNeeded;
            }

            return result;
        }

        public void ExtractSaltAndDoubleHashedPassword(byte[] passwordData, out byte[] legacySalt, out byte[] encodedPassword)
        {
            // 0xFF + legacy salt length + legacy salt + legacy password hash encoded by ASP.NET Core Identity
            var legacySaltLength = BitConverter.ToInt32(passwordData, 1);
            var passwordStartIndex = 5 + legacySaltLength;

            legacySalt = passwordData[5..passwordStartIndex];
            encodedPassword = passwordData[passwordStartIndex..];
        }

        private string HashPasswordUsingLegacyAlgorithm(string providedPassword, byte[] legacyPasswordSalt)
        {
            // NOTES:
            // We support HMACSHA256 and hashed password format which is the default
            // Adjust to your needs if you use older algorithm

            // key is 64 bytes long - salt is 16 bytes, so it is repeated 4 times
            byte[] key = [.. legacyPasswordSalt, .. legacyPasswordSalt, .. legacyPasswordSalt, .. legacyPasswordSalt];
            var hashData = Encoding.Unicode.GetBytes(providedPassword);

            var hashAlgorithm = new HMACSHA256(key);
            var result = hashAlgorithm.ComputeHash(hashData);
            return Convert.ToBase64String(result);
        }

        public string BuildDoubleHashedPassword(AppUser user, string legacySalt, string legacyPasswordHash)
        {
            // encode the legacy hash using ASP.NET Code Identity
            var encodedPassword = HashPassword(user, legacyPasswordHash);

            // build the password data structure
            // 0xFF + legacy salt length + legacy salt + encoded password
            var legacySaltBytes = Convert.FromBase64String(legacySalt);
            var encodedPasswordBytes = Convert.FromBase64String(encodedPassword);
            return Convert.ToBase64String(
            [
                0xFF,
                ..BitConverter.GetBytes(legacySaltBytes.Length),
                ..legacySaltBytes,
                ..encodedPasswordBytes
            ]);
        }

    }
}
