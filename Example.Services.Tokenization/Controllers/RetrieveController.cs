using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using Example.Library.TokenStorage;
using Example.Services.Tokenization.Filters;

namespace Example.Services.Tokenization.Controllers
{
    /// <summary>
    /// Controller servicing the '/api/retrieve/{GUID}' endpoint
    /// </summary>    
    public class RetrieveController : ApiController
    {
        /// <summary>
        /// HTTP GET operation to retrieve the decrypted string, original string value.
        /// </summary>
        /// <param name="token">The token returned by the '/api/store' endpoint
        /// as a URL friendly string.</param>
        /// <returns>Decrypted string that was stored encrypted.</returns>
        [HttpGet]
        [ResponseType(typeof(string))]
        [ExampleCustomAuthorizationFilter]
        public IHttpActionResult Get(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return BadRequest();

            var storageProviderName = ConfigurationManager.AppSettings["ITokenStorageProviderName"];
            ITokenStorageProvider storage = StorageProviderFactory.Create(storageProviderName);

            if(storage == null)
                return InternalServerError(new ArgumentException("Storage provider not found."));

            //try to get the item
            var value = storage.Read(token);

            if (string.IsNullOrWhiteSpace(value))
                return NotFound();

            return Ok(value);
        }
    }
}
