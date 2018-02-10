namespace Example.Library.AesEncryption
{
    public interface IEncryptionProvider
    {
        string Encrypt(string plainText, string passPhrase, string salt);

        string Decrypt(string cypherText, string passPhrase, string salt);
    }
}
