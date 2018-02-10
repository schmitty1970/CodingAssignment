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

## My Design Approach and Considerations
