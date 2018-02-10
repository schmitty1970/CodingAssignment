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

## Notes on Security of this project
* Calls from clients of this API should ALWAYS use TLS, in fact in production this API should be configured to ONLY accept HTTPS connections.  
* The authorization of client calls in this implemenation are for example purposes only.  As stated in code comments, one could use Thinktecture's Identity Server OAuth/OpenID Connect to implement a more secure inter-machine communication using the client credentials grant.
* The file storage implemenation in this project is also for example and/or testing purposes.  In a production implemenation, the encrypted data and associated meta data should be stored in some type of database that also contains security featres to further secure the data and access to the data.
* Regarding the impelmentation of the AES encryption/dycrption.  I wanted to demonstrate the use the .NET crypto classes in this assignment.  One could use an 'audited' implemenation such as the Inferno Crypto Library (http://securitydriven.net/inferno/)  
