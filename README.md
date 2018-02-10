# Coding Assignment
Public repository for interview coding assignment review.

## Requirements Provided
A REST API to do basic data tokenization written in C#.  This API should have two endpoints, store and retrieve.
1. Store endpoint
* Accepts a piece of data
* Encrypts it with appropriate symmetric algorithm
* Generates a GUIDE (I assumed Evan meant GUID) to be used as a key for looking up the data later
* Stores ciphertext and any necessary metadata
* Return the key
2. Retrieve endpoint
* Accepts a key
* Looks up and decrypts the data
* Returns the data

You can use any data storage you like.  Lastly, create it as a project in a public GitHub, then share the link once you are done.

## My Design Goals
* Follow REST architectural style, making use of URI resources, HTTP verbs, HTTP return codes, and authentication headers
* Make sure that the GUID tokens used are URL friendly so they are safely used in HTTP GET requests
* Authorize calls made to the endpoints (example implementation only)
* Use strong AES encryption algorithm
* Use derived key and init vector based on pass phrase and random salt
* Design flexible/extensible storage architecture. Allow other storage implementations to be added based on interface.  Allow each storage implementation to specify its encryption provider.
* Consider security throughout, even this is only a coding exercise
* Comment code to explain my decisions, what changes I would have made it this was a production ready implementation, etc.

## Layout of this solution
**Example.Services.Tokenization** - the ASP.NET Web API 2 that implements the REST api defined in the requirements
**Example.Library.AesEncryption** - a .NET Framework class library containing the encryption and related code.
**Example.Library.TokenStorage** - a .NET Framework class library containing the storage code.  File storage is the only implementation for now.
**Example.Services.Tokenization.Tests** - a unit test project for testing code in the class library projects.
**Example.Console.TestHarness** - a .NET Framework console application that tests the system from a client applications perspective.

## Notes on Security of this project
* Calls from clients of this API should ALWAYS use TLS, in fact in production this API should be configured to ONLY accept HTTPS connections.  
* The authorization of client calls in this implementation are for example purposes only.  As stated in code comments, one could use Thinktecture's Identity Server OAuth/OpenID Connect to implement a more secure inter-machine communication using the client credentials grant.
* The file storage implementation in this project is also for example and/or testing purposes.  In a production implementation, the encrypted data and associated meta data should be stored in some type of database that also contains security features to further secure the data and access to the data.
* Regarding the implementation of the AES encryption/decryption.  I wanted to demonstrate the use the .NET crypto classes in this assignment.  One could use an 'audited' implementation such as the Inferno Crypto Library (http://securitydriven.net/inferno/)  

## Additional Notes
* I would have configured a DI container, such as AutoFac, for use in the API to inject dependencies like the storage provider.
* I would still add a logging mechanism, such as Log4Net.

## Notes on running this project locally
* There are some (limited) unit tests Example.Services.Tokenization.Tests that can be run
* You can set the Example.Services.Tokenization web api as the 1st startup project and the Example.Console.TestHarness as the second so that when you run or debug the local VS web server is ready before the test harness runs.
* The application writes it's file based db to the C drive, you might have to adjust your ACL's or location that the file is written to, depending on your local configuration.
