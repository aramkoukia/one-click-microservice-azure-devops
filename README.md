# one-click-microservice-azure-devops
This project is supposed to help you spin up new microservices with project templates (.Net Core C#, F# and node), build and release pipelines in Azure DevOps

* user enters:
1. the Project Name (DevOps Project or Domain (Bouonded Context?))
2. Service Name
3. Repository Name
4. Template Type (C# Core, F# Core, node)

* we create a DevOps project
* we add the Repo in it
* we make a template project of user's choice and commit to the repo (could have instrumentation, swagger, etc all ready to go)
* we make the build and release pipelines

## Inspired by the great work in the following project:
https://github.com/andreschaffer/one-click-microservice

Would like to make this happen, but with Microsoft Azure eco-system

...


## Additional things to do?
* a centralized API documentation like Swagger, so the API signature can be linted! (as if one person is writing all the APIs) checking for standards and conventions
* then some engine to build Mock APIs based on the specification and the template (C# Core, F# Core, node) of choice
* then all the above steps to make the pipelines etc...
* a tool to manage a list of microservices from before they even exist (planning/documentation), thru development, into production, monitoring... all the way to end-of-life
* can this help 

### References:
* Azure DevOps REST API: https://docs.microsoft.com/en-us/rest/api/azure/devops/?view=azure-devops-rest-5.0
* How to create Custom Template with `dotnet new` https://docs.microsoft.com/en-us/dotnet/core/tutorials/create-custom-template
