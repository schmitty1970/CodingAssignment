using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Example.Library.AesEncryption;
using Newtonsoft.Json;

namespace Example.Library.TokenStorage
{
    /// <summary>
    /// An example of how one could store object, or arrays of objects in a file.
    /// This would NOT be the storage mechanism in a production implementation, this
    /// is for the coding assignment only.
    /// </summary>
    public sealed class FileStorageProvider : AbstractStorageProvider, ITokenStorageProvider
    {
        //Again, just an example of a location that the JSON file could be stored at.
        //This location would need to have the proper ACL's assigned to allow read/write/modify
        //for the identity (AppPool) running the Web API...
        private const string FilePath = @"C:\file_db.json";
    
        public IEncryptionProvider EncryptionProvider => new AesManagedProvider();

        public string Create(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("Value cannot be null or empty.", nameof(plainText));

            var tokenStorageItem = new TokenStorageItem();

            var salt = SaltUtil.GenerateSecureRandomSalt();
            var encrypted = EncryptionProvider.Encrypt(plainText, KeyVaultUtil.GetSecureSecretFromVault(), salt);

            if (encrypted != null)
            {
                tokenStorageItem = new TokenStorageItem
                {
                    Token = CreateTokenGuid(),
                    SaltValue = salt,
                    EcnryptedValue = encrypted,
                    CreateDateTime = DateTime.UtcNow,
                    UpdatedDateTime = DateTime.UtcNow
                };
            }

            //write or append to json file
            WriteAppendDataItem(tokenStorageItem);

            return tokenStorageItem.Token;
        }

        public string Read(string token)
        {
            var data = LoadJsonFromFile();

            if (data == null || data.Count == 0)
                return null;
           
            var tokenStorageItem = data.FirstOrDefault(i => i.Token.Equals(token));

            if (tokenStorageItem == null)
                return null;

            if (string.IsNullOrWhiteSpace(tokenStorageItem.EcnryptedValue) ||
                string.IsNullOrWhiteSpace(tokenStorageItem.SaltValue))
                return null;

            var decrypted = EncryptionProvider.Decrypt(tokenStorageItem.EcnryptedValue, 
                                                        KeyVaultUtil.GetSecureSecretFromVault(),
                                                        tokenStorageItem.SaltValue);

            return decrypted;
        }

        #region Private Helper Methods
        /// <summary>
        /// Helper method to read data from the JSON file.
        /// </summary>
        /// <returns>List of TokenStorageItem object or an empty list if file does not exist</returns>
        private List<TokenStorageItem> LoadJsonFromFile()
        {
            var data = new List<TokenStorageItem>();

            //see if data already exists, if not return initialized list
            if (!File.Exists(FilePath))
                return data;

            //load existing data
            using (var r = new StreamReader(FilePath))
            {
                var json = r.ReadToEnd();
                data = JsonConvert.DeserializeObject<List<TokenStorageItem>>(json);
            }

            return data;
        }

        /// <summary>
        /// Helper method to write the contents of the list of TokenStorageItem back to the file system.
        /// </summary>
        /// <param name="item">TokenStorageItem item to add to the list.</param>
        private void WriteAppendDataItem(TokenStorageItem item)
        {
            var data = LoadJsonFromFile();
            data.Add(item);

            //convert to json string 
            var json = JsonConvert.SerializeObject(data.ToArray(), Formatting.Indented);

            //write file, or overwrite if it already exists
            File.WriteAllText(FilePath, json);
        }
        #endregion
    }
}