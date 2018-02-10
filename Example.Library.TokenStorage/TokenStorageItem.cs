using System;

namespace Example.Library.TokenStorage
{
    /// <summary>
    /// Class to serve as the data container for token, encrypted value
    /// and meta data to store.
    /// </summary>
    public class TokenStorageItem
    {
        /// <summary>
        /// Token (GUID) that maps to the encrypted value
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Encrypted string value
        /// </summary>
        public string EcnryptedValue { get; set; }

        /// <summary>
        /// Salt value used during encryption, need to decryption
        /// </summary>
        public string SaltValue { get; set; }

        /// <summary>
        /// Created date and time in UTC
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Updated date and time in UTC
        /// </summary>
        public DateTime UpdatedDateTime { get; set; }
    }
}
