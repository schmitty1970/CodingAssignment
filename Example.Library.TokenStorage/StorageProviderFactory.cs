using System;
using System.Runtime.Remoting;

namespace Example.Library.TokenStorage
{
    /// <summary>
    /// Simple factory method to create the storage provider
    /// by type name, that is passed in using reflection.
    /// </summary>
    public static class StorageProviderFactory
    {
        public static ITokenStorageProvider Create(string name)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentException(nameof(name));

            ITokenStorageProvider provider = null;

            try
            {
                ObjectHandle handle = Activator.CreateInstance("Example.Library.TokenStorage", name);
                provider = (ITokenStorageProvider)handle.Unwrap();
            }
            catch (Exception)
            {
                //TODO: should provide some loggging, maybe Log4Net or another library, out of scope for this assignment
            }
            

            return provider;
        }
    }
}
