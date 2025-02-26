# Blog
A RESTful API for managing a simple blogging platform.  

### Requirements
This project was based on the following requirements:
- Concept of Minimal API's;
- KISS (Keep It Simple, Stupid) principle;
- SOLID Principles;
- Clean Code;


### Installation

To run the project you need to have the following "programs/frameworks" installed:
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download);
- [Docker](https://docs.docker.com/engine/install/);

### Usage
- Via Docker:
  - Open the Terminal in the folder where the docker-compose file is located (root folder).
  - Type the command: "docker-compose build" and wait for the build;
  - Type the command: "docker-compose up" to publish and run the image; 
  
>__Note__:
The project is configured to open on port [http:32774], if you are already using this port, you will need to change the port in the docker-compose file.

- Via Visual Studio:
  - Open the project in Visual Studio;
  - Set the project "http" or "https" as the startup project;
  - Run the project;

>__Note__:
The project is configured to open on port [http:32774], if you are already using this port, you will need to change the port in the launchSettings file.

### Tests
- Via Browser:
  - Enter the address: [http://localhost:32774/api/posts](http://localhost:32774/api/posts);
  
- Via Blog.http:
  - At the root of the Blog project, there is a file called Blog.http.  
  It contains tests for all requests.
  - Open the file and click on the Send Request link above the requests.

- Via Scalar:
- Enter the address: [http://localhost:32774/scalar/v1](http://localhost:32774/scalar/v1);
- On the Test Request Page, check if the address is pointed to http://localhost:32774.

Hope you enjoy it! :relaxed: :coffee: :sunglasses:
