using System.Security.Cryptography;
using System.Text;

namespace Kutiyana_Memon_Hospital_Api.Helpers
{ 
        public static class PasswordHelper
        {
            public static (string Hash, string Salt) HashPassword(string password)
            {
                using (var hmac = new System.Security.Cryptography.HMACSHA256())
                {
                    var salt = Convert.ToBase64String(hmac.Key); 
                    var hash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
                    return (hash, salt);
                }
            }

            public static bool VerifyPassword(string password, string storedHash, string storedSalt)
            {
                var key = Convert.FromBase64String(storedSalt);

                using (var hmac = new System.Security.Cryptography.HMACSHA256(key))
                {
                    var computedHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
                    return computedHash == storedHash;
                }
            }
        }
     
}
