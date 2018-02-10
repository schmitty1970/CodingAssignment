using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Example.Library.AesEncryption
{
    /// <summary>
    /// A wrapper around the managed version of the .NET AES implementation.
    /// 
    /// 
    /// Note: this is not FIPS compliant.  If you need a FIPS complian version
    /// you would need to implement one using the AesCryptoServiceProvider class.
    /// 
    /// For a more robust and audited implementation, One might consider using a component such as
    /// the Inferno Crypto Library (http://securitydriven.net/inferno/).  However,
    /// for this interview coding task I wanted to demonstrate using the built in
    /// .NET crypto classes.
    /// </summary>
    public class AesManagedProvider : IEncryptionProvider
    {
        /// <summary>
        /// Excrypts the plain text value usisng the secret pass phase and non-secret salt 
        /// to generate the key and initialization values.  This allows the use of any size
        /// pass phrase eventhough AES only allows specific key sizes.
        /// </summary>
        /// <param name="plainText">Data to encrypt</param>
        /// <param name="passPhrase">Secret pass phrase that was used for encryption</param>
        /// <param name="salt">Salt plainText that was used during the encryption</param>
        /// <returns>Encrypted cypher of the original data</returns>
        public string Encrypt(string plainText, string passPhrase, string salt)
        {
            try
            {
                //use password-based key derivation , PBKDF2, using a pseudo-random number generator based on HMACSHA1
                DeriveBytes rgb = new Rfc2898DeriveBytes(passPhrase, Encoding.Unicode.GetBytes(salt));

                //create the algorithm and generate the key and initialization vector
                AesManaged algorithm = new AesManaged();
                byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3); //get 32 bytes, as key size defaults to 256
                byte[] rgbIv = rgb.GetBytes(algorithm.BlockSize >> 3); //get 16 bytes, as block size is always 128 in AesManaged

                //create the encryptor based on the derived key and init vector
                ICryptoTransform transform = algorithm.CreateEncryptor(rgbKey, rgbIv);

                //write to the a memory stream buffer using the CryptoStream and the encryptor,
                //using blocks will flush and dispose of the streams
                using (MemoryStream buffer = new MemoryStream())
                {
                    using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(stream, Encoding.Unicode))
                        {
                            writer.Write(plainText);
                        }
                    }

                    //return the encrypted plainText as a Base64 encoded string for storage
                    return Convert.ToBase64String(buffer.ToArray());
                }
            }
            catch (Exception)
            {
                //fail safely
                return null;
            }
        }

        /// <summary>
        /// Method to decrypt encrypted cypher text using the same secret pass phrase 
        /// and non-secret salt value used for encryption.
        /// </summary>
        /// <param name="cypherText">Data to be decrypted</param>
        /// <param name="passPhrase">Secret pass phrase that was used for encryption</param>
        /// <param name="salt">Salt plainText that was used during the encryption</param>
        /// <returns>Original clear text data</returns>
        public string Decrypt(string cypherText, string passPhrase, string salt)
        {
            try
            {
                //use password-based key derivation , PBKDF2, using a pseudo-random number generator based on HMACSHA1
                DeriveBytes rgb = new Rfc2898DeriveBytes(passPhrase, Encoding.Unicode.GetBytes(salt));

                //create the algorithm and generate the key and initialization vector
                AesManaged algorithm = new AesManaged();
                byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);//get 32 bytes, as key size defaults to 256
                byte[] rgbIv = rgb.GetBytes(algorithm.BlockSize >> 3);//get 16 bytes, as block size is always 128 in AesManaged

                //create the decryptor based on the derived key and init vector
                ICryptoTransform transform = algorithm.CreateDecryptor(rgbKey, rgbIv);

                //write to the a memory stream buffer using the CryptoStream and the decryptor,
                //using blocks will flush and dispose of the streams
                using (MemoryStream buffer = new MemoryStream(Convert.FromBase64String(cypherText)))
                {
                    using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.Unicode))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception)
            {
                //fail safely
                return null;
            }
        }        
    }
}
