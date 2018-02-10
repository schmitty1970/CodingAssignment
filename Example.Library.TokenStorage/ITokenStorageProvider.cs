using System;
using Example.Library.AesEncryption;

namespace Example.Library.TokenStorage
{
    public interface ITokenStorageProvider
    {
        #region Interface Methods
        /// <summary>
        /// Given a valid token, the Read method looks up the corresponding
        /// value from storage, decrypts and return the plain text value. 
        /// </summary>
        /// <param name="token">Guid string token for the value to look up.</param>
        /// <returns>Original plain text value that was stored.</returns>
        string Read(string token);

        /// <summary>
        /// Takes the passed in string parameter, encrypts it, creates
        /// a guid token for it and stores it.
        /// </summary>
        /// <param name="plainText">Plain text value to be encrypted and stored.</param>
        /// <returns>Guid string token that can be used to retrieve, update or delete 
        /// the value from storage</returns>
        string Create(string plainText);


        #region Additional Non-Used Methods (Example only)
        ///////////////////////////////////////////////////////////////////////
        /// The following methods were NOT called for int the assignment's
        /// requirements.  However, I have noted them in this interface as
        /// thry might be useful additions.
        ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Given a valid token, and a new plain text value to replace the
        /// old encrypted value, looks up the item, encrypts the new value
        /// and replaces the old encrypted value with the new one and then
        /// stores the new value using the same token.
        /// </summary>
        /// <param name="token">Guid string token for the value to look up.</param>
        //// <param name="value">New, plain text value to be encrypted and stored.</param>
        /// <returns>Boolean true if the update succeeded, false otherwise.</returns>
        //bool Update(Guid token, string value);

        /// <summary>
        /// Given a valid token, removes the item from storage.
        /// </summary>
        /// <param name="token">Guid string token for the value to look up.</param>
        /// <returns>Boolean true if the update succeeded, false otherwise.</returns>
        //bool Delete(Guid token);
        #endregion

        #endregion

        #region Interface  Properties
        //Encryption provider to use.  Allows, the concrete implementations of
        //of this interface to define which IEncryptionProvider it uses.
        //Of course for this coding assignment, there is only one at this point
        //in time...
        IEncryptionProvider EncryptionProvider { get; }
        #endregion

    }
}