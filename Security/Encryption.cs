using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Configuration;

namespace API.Security
{
    public class Encryption
    {
        public static string ComputeHash(string password, string hash)
        {
            int hashByteSize = Int32.Parse(ConfigurationManager.AppSettings["CRPT_HASH_SIZE"]);
            int iterations = Int32.Parse(ConfigurationManager.AppSettings["CRPT_HASH_ITERATIONS"]);
            byte[] salt = Convert.FromBase64String(hash);

            Rfc2898DeriveBytes hashGenerator = new Rfc2898DeriveBytes(password, salt);

            hashGenerator.IterationCount = iterations;

            return Convert.ToBase64String(hashGenerator.GetBytes(hashByteSize));
        }

        public static string GenerateSalt()
        {
            int saltByteSize = Int32.Parse(ConfigurationManager.AppSettings["CRPT_SALT_SIZE"]);
            byte[] salt = new byte[saltByteSize];

            RNGCryptoServiceProvider saltGenerator = new RNGCryptoServiceProvider();

            saltGenerator.GetBytes(salt);

            return Convert.ToBase64String(salt);
        }
    }
}
