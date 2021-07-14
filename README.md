# Assignment Scalable Web

### Running the code

After cloning the repo, simply open the project with visual studio and click play!
It's possible to execute via command line too, folllowing some steps:
- 1: Navigate to the project folder ```scr/ScalableDiff/```
- 2: Open a terminal and run the following command: ```dotnet run ScalableDiff.csproj```
- 3: Navigate to ```https://localhost:5001/swagger``` to acess the api documentation.

### Unit Tests

There are 26 unit tests available that cover most of the implementation.
I didn't implement the Models and AutoMapper profiles tests.
To run the integration tests, use the following steps:

- 1: Navigate to the project folder ```scr/ScalableDiff.UnitTests/```
- 2: Open a terminal and run the following command: ```dotnet test ScalableDiff.UnitTests.csproj```
- 3: Watch the output.

### Integration Tests

There are 6 integration tests available that cover some api scenarios (positive/negative).
To run the integration tests, use the following steps:

- 1: Navigate to the project folder ```scr/ScalableDiff.IntegrationTests/```
- 2: Open a terminal and run the following command: ```dotnet test ScalableDiff.IntegrationTests.csproj```
- 3: Watch the output.

## About the solution & Overview

A simple poc for text diffing. 

The project follows a simplified version of a DDD style project. 
I did it as simple as possible to match the criterias specified in the assignement.

For breviety I using some patterns like:
- Method Factories: using in the Models/Value objects Models), since the models are so simple I didn't write "full factories" in the application.
- Chain of responsability: using to execute the diffing process (could be like a decorator too)

There are documentation by the code, also I tried to be consitent with the naming/meaning of the methods so It's easy to navigate in the solution.

### Tecnologies
- ASP.NET Core 5
- xUnit, Moq
- AutoMapper
- Domain Driven Style implementation
- Dependency Injection using the default DI container.
- Swagger for basic api documentation.


### Application
This contains all application logic and is dependent on the domain layer and infrastructure.


### Domain
This will contain all entities, enums, interfaces, types and logic specific to the domain layer.


### Infrastructure
This project is suposed to contain classes for accessing external resources such as databases, web services, smtp, and so on. 
Here it's possible to find a Store implenentation specified in the domain layer, that works with in memory data.


## WebApi
This is a web api application based on ASP.NET 5.0.x. Here it will define the diff controller and register the dependency graph as well.
I've added the .net versioning lib in order the enable the endpoint/action versioning, but implemented a simple controller only.
