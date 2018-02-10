using Example.Library.TokenStorage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Example.Services.Tokenization.Tests
{
    [TestClass]
    public class StorageProviderTests
    {
        [TestMethod]
        public void FileStorage_CreateRetrieve_Test()
        {
            var plainText = "Some test data to save to a file, single item.";
            ITokenStorageProvider fileProvider = new FileStorageProvider();

            var token = fileProvider.Create(plainText);
            Assert.IsNotNull(token);

            var decrypted = fileProvider.Read(token);
            Assert.AreEqual(plainText, decrypted);
        }

        [TestMethod]
        public void FileStorage_CreateRetrieveMultipleRecords_Test()
        {
            var plainText1 = "Another bit of data to save";
            var plainText2 = "Another bit of data to save, the second one to save";
            ITokenStorageProvider fileProvider = new FileStorageProvider();

            var token1 = fileProvider.Create(plainText1);
            Assert.IsNotNull(token1);

            var token2 = fileProvider.Create(plainText2);
            Assert.IsNotNull(token1);

            var decrypted = fileProvider.Read(token1);
            Assert.AreEqual(plainText1, decrypted);

            var decrypted2 = fileProvider.Read(token2);
            Assert.AreEqual(plainText2, decrypted2);
        }
    }
}
