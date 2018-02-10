namespace Example.Library.AesEncryption
{
    /// <summary>
    /// EXAMPLE ONLY
    /// In a "real" implementation, we would want to retrieve
    /// the secret pass phrase from some secure location.  Possibly 
    /// something like Azure Key Vault...
    /// 
    /// The pass phrase could also be placed in the web.config file and
    /// encrypted using aspnet_regiis.exe.
    /// 
    /// IMO, it would be better to use an external service to get it from 
    /// in a production environment.
    /// </summary>
    public static class KeyVaultUtil
    {
        public static string GetSecureSecretFromVault()
        {
            //TODO: FOR EXAMPLE ONLY, DO NOT STORE SECRETS IN CODE...
            return "KUc[mNmmN6P%<7Y.j=T/8e:Ma_Zt7Uw^";
        }        
    }
}
