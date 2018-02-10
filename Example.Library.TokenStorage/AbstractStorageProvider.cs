using System;

namespace Example.Library.TokenStorage
{
    public abstract class AbstractStorageProvider
    {
        #region Public Methods
        /// <summary>
        /// Creates a new guid, and converts it to a URL safe string representation,
        /// that is returned as the token for the encrypted value.
        /// </summary>
        /// <returns>Guid as string, safe for use in URL</returns>
        public string CreateTokenGuid()
        {
            return Guid.NewGuid().ToString("N");
        }

        #endregion
    }
}
