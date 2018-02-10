using Example.Library.AesEncryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Example.Services.Tokenization.Tests
{
    [TestClass]
    public class EncryptionProviderTests
    {        
        [TestMethod]
        public void EncryptDecrypt_WithSpecialChars_Test()
        {
            var passPhrase = KeyVaultUtil.GetSecureSecretFromVault();
            var data = "Some secret user's data¡";
            var salt = SaltUtil.GenerateSecureRandomSalt();

            AesManagedProvider crypto = new AesManagedProvider();
            string encrypted = crypto.Encrypt(data, passPhrase, salt);
            string decrypted = crypto.Decrypt(encrypted, passPhrase, salt);
            Assert.AreEqual(data, decrypted);
        }

        [TestMethod]
        public void EncryptDecrypt_CC_Number_Test()
        {
            var passPhrase = KeyVaultUtil.GetSecureSecretFromVault();            
            var data = "4111111111111111";
            var salt = SaltUtil.GenerateSecureRandomSalt();

            AesManagedProvider crypto = new AesManagedProvider();
            string encrypted = crypto.Encrypt(data, passPhrase, salt);
            string decrypted = crypto.Decrypt(encrypted, passPhrase, salt);
            Assert.AreEqual(data, decrypted);
        }

        [TestMethod]
        public void EncryptDecrypt__Test()
        {
            var passPhrase = KeyVaultUtil.GetSecureSecretFromVault();
            var data = "4111111111111111";
            var salt = SaltUtil.GenerateSecureRandomSalt();

            AesManagedProvider crypto = new AesManagedProvider();
            string encrypted = crypto.Encrypt(data, passPhrase, salt);
            string decrypted = crypto.Decrypt(encrypted, passPhrase, salt);
            Assert.AreEqual(data, decrypted);
        }

        [TestMethod]
        public void EncryptDecrypt_LargeText__Test()
        {
            var passPhrase = KeyVaultUtil.GetSecureSecretFromVault();
            var data = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                       "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure " +
                       "dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                       "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
            
            var salt = SaltUtil.GenerateSecureRandomSalt();

            AesManagedProvider crypto = new AesManagedProvider();
            string encrypted = crypto.Encrypt(data, passPhrase, salt);
            string decrypted = crypto.Decrypt(encrypted, passPhrase, salt);
            Assert.AreEqual(data, decrypted);
        }

        [TestMethod]
        public void Decrypt_FailOnKey_Test()
        {
            var passPhrase = KeyVaultUtil.GetSecureSecretFromVault();
            var data = "4111111111111111";
            var salt = SaltUtil.GenerateSecureRandomSalt();

            AesManagedProvider crypto = new AesManagedProvider();
            string encrypted = crypto.Encrypt(data, passPhrase, salt);
            string decrypted = crypto.Decrypt(encrypted, "sdufsdkj3434^87", salt);
            Assert.IsNull(decrypted);
        }

        [TestMethod]
        public void Decrypt_FailSalt_Test()
        {
            var passPhrase = KeyVaultUtil.GetSecureSecretFromVault();
            var data = "4111111111111111";
            var salt = SaltUtil.GenerateSecureRandomSalt();

            AesManagedProvider crypto = new AesManagedProvider();
            string encrypted = crypto.Encrypt(data, passPhrase, salt);
            string decrypted = crypto.Decrypt(encrypted, passPhrase, "sldfjsdlksld");
            Assert.IsNull(decrypted);
        }

    }
}
