using System;
using System.Configuration;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Example.Library.TokenStorage;
using Example.Services.Tokenization.Filters;

namespace Example.Services.Tokenization.Controllers
{
    /// <summary>
    /// Controller servicing the '/api/store' endpoint
    /// </summary>
    public class StoreController : ApiController
    {
        /// <summary>
        /// HTTP POST operation to take a plain text input, encrypt it, store it and return a Guid token that
        /// can be used to retrieve it
        /// </summary>
        /// <param name="plainText">The data to encrypt and store.</param>
        /// <returns>A Guid in string format, serving as the token associated with the value</returns>
        [HttpPost]
        [ResponseType(typeof(string))]
        [ExampleCustomAuthorizationFilter]
        public IHttpActionResult Post([FromBody]string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return BadRequest(); //http 400-bad request


            var storage = GetStorageProvider();

            if (storage == null)
                return CreateCreationError();

            //calling create will encrypt, create a GUID token, and store the value
            var token = storage.Create(plainText);

            if(string.IsNullOrWhiteSpace(token))
                return InternalServerError(new ArgumentException("Could not tokenize value."));

            //return http 201-created with the URI location, 
            //(how to request this thing we just created),
            //and the actual content, which is the GUID token 
            //for the encrypted value
            return Created<string>($"{Request.RequestUri}/{token}", token);
        }


        #region Private Methods
        private ITokenStorageProvider GetStorageProvider()
        {
            //get the storage provider, based on config setting 
            //Note: more robust implementation would probably use Dependency Injection, for now using a factory
            var storageProviderName = ConfigurationManager.AppSettings["ITokenStorageProviderName"];
            ITokenStorageProvider storage = StorageProviderFactory.Create(storageProviderName);

            return storage;
        }

        private IHttpActionResult CreateCreationError()
        {
            return InternalServerError(new ArgumentException("Storage provider not found."));
        }
        #endregion


        #region Additional Methods
        ///////////////////////////////////////////////////////////////////////
        /// The following operations were NOT called for int the assignment's
        /// requirements.  However, I have written implementations and have 
        /// commented them out for now, so that I am not adding to the possible
        /// attack serface of this API when not required.  Typically, in a 
        /// production system, this code would NOT even be included... for example 
        /// only in this coding assignment.
        /// See accompaning note in the ITokenStorageProvider interface...
        ///////////////////////////////////////////////////////////////////////


        //[HttpPut]
        //[ResponseType(typeof(bool))]
        //public IHttpActionResult Put(string token, [FromBody]string plainText)
        //{
        //    var storage = GetStorageProvider();

        //    if (storage == null)
        //        return CreateCreationError();

        //    //try to make the update, GUID will remain the same
        //    var result = storage.Update(token, plainText);

        //    return StatusCode(result ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
        //}

        //[HttpDelete]
        //[ResponseType(typeof(bool))]
        //public IHttpActionResult Delete(string token)
        //{
        //    var storage = GetStorageProvider();

        //    //try to delete the item
        //    return Ok(storage != null && storage.Delete(token));
        //}
        #endregion
    }
}
