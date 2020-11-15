# Candidate API
An API to find the best candidate for a position based on skill set. This API has two endpoints that are available to the user a GET endpoint to retrieve a candidate based on skills provided and a POST endpoint to add candidates to the system.

## Setting up

To start operation of the API you can do this through `dotnet run` in the location you have cloned the GIT repository into and this will start the API.

 I have also provided a docker compose file if you wish to bring the API up in a container. This method can be used with the following command `docker-compose up --build` from the directory you have cloned the repository into.

## Retrieving a candidate (GET)

When making a request to retrieve a candidate a request like this should be used:

```
curl --location --request GET 'http://localhost:5000/candidates?skills=javascript,%20es6,%20nodejs,%20express' \
--header 'Content-Type: application/json'
```

You may add additonal skills seperating them with the `","` character. If a candidate is not found with the skills requested or no candidates exist in the data store then a 404 not found HTTP code will be returned. If the request is malformed or the skills are not provided then a 400 bad request HTTP code will be returned. 

## Storing a candidate (POST)

In order to store a candidate a request like this should be used:

```
curl --location --request POST 'http://localhost:5000/candidates' \
--header 'Content-Type: application/json' \
--data-raw '{
    "id": "ae588a6b-4540-5714-bfe2-a5c2a65f547a",
    "name": "Jimmy Coder",
    "skills": [
      "java",
      "javascript"
    ]
}'
```

You can add additonal skills as you wish to the candidate and they will be included when stored. If the request body is not provided then a 400 bad request HTTP code will be returned. 

## Testing

There are some example tests of the main `CandidateService` included in the project and they can be ran with the `dotnet test` command. Due to time constraints I have only provided a limited number of tests however in a full project I would included tests covering as much of the code base as possible along with acceptance tests usually in a javascript testing framework to test end to end operation. 

## Documentation

There has been XML documentation used in the code base to assist along with a swagger endpoint that would be available at `http://localhost:5000/swagger/index.html`. You can use the swagger endpoint to see the endpoints available and the possible results along with being able to make test requests to the API.

## Logging

I have added some rudimentaty logging to the console when a candidate is stored and to show when a candidate can't be matched the reason and to also show any errors that might come from the data store. I have also included some request logging middleware to log each request as it comes into the API as this can be useful for debugging the requests coming in and the responses that are being returned. 

In a full project I would have used the Serilog nuget package to set up logging to an appropiate logging provider and ensure sufficent logging was throughout the project. 

## Assumptions

As per the document provided I have made the following main assumptions:

* Each candidate has a unique Id and there will never be a situation where the Id is duplicated
* Id is any string from 1 to 100 characters;
* Name is any string from 1 to 100 characters;
* Skills is an array of strings from 1 to 100 characters, being letters, numbers or hyphens ([a-zA-Z0-9-]+), with a maximum of 10,000 elements); elements in the array or in query strings are not duplicated (there is no [ "skill1", "skill2", "skill1" ]).

In a full project I would back these assumptions up with fluent validation to check these conditions and return an approrpiate error code o the user if they were broken. As instructed in the document provided I have assumed that they will be true in this case.
