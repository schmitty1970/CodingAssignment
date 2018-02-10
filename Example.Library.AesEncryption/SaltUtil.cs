using System.Security.Cryptography;
using System.Text;

namespace Example.Library.AesEncryption
{
    public static class SaltUtil
    {
        /// <summary>
        /// Generate a cryptographic random salt to be used 
        /// during the encryption process
        /// </summary>
        public static string GenerateSecureRandomSalt()
        {
            var random = new RNGCryptoServiceProvider();
            byte[] salt = new byte[32];
            random.GetNonZeroBytes(salt);
            return Encoding.Unicode.GetString(salt);
        }
    }
}
