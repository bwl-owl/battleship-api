# battleship-api

REST API written in C# .NET 6 for simulating a Battleship game.

Deployed on Azure at https://battleshipapiofx.azurewebsites.net/swagger/index.html

## Features

- Create game board
- Add battleship to game board
- Attempt attack on game board

## Local setup/running instructions

### Pre-requisites

- Visual Studio 2022
- .NET 6

### Steps

1. Run `dotnet restore`.
2. Run API in Visual Studio (IIS Express).
3. Browser should open to show Swagger documentation; use this or tool such as Postman to execute your requests.
   - Postman collection and environment variable files can be found under the [postman](postman) folder.

## Future adds

- Address TODOs in code
- Linting
- Integration/E2E tests
- Centralise content/error messages management
