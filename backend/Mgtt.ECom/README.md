# Mgtt.ECom

## Table of Contents

- [Summary](#summary)
- [References](#references)
- [Getting Started](#getting-started)
- [Author](#author)

## Summary

A simple backend for an e-commerce platform utilizing DDD principles based on an existing cookiecutter template

## References

- [Design a DDD-oriented microservice](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)

## Getting Started

### Preconditions

- Preferably use the [dev container feature in VS Code IDE](https://code.visualstudio.com/docs/devcontainers/containers) to set up a development container. 
- If you plan to use Visual Studio as IDE for development (debugging, profiling), consider [installing it here](https://visualstudio.microsoft.com/). Afterward run the following commands to set up the solution file and associate the project files:

```sh
dotnet new sln -n Mgtt.ECom

dotnet sln Mgtt.ECom.sln add ./src/Mgtt.ECom.Application/Mgtt.ECom.Application.csproj
dotnet sln Mgtt.ECom.sln add ./src/Mgtt.ECom.Domain/Mgtt.ECom.Domain.csproj
dotnet sln Mgtt.ECom.sln add ./src/Mgtt.ECom.Infrastructure/Mgtt.ECom.Infrastructure.csproj
dotnet sln Mgtt.ECom.sln add ./src/Mgtt.ECom.Persistence/Mgtt.ECom.Persistence.csproj
dotnet sln Mgtt.ECom.sln add ./src/Mgtt.ECom.Web/Mgtt.ECom.Web.csproj

dotnet sln Mgtt.ECom.sln add ./test/Mgtt.ECom.ApplicationTest/Mgtt.ECom.ApplicationTest.csproj
dotnet sln Mgtt.ECom.sln add ./test/Mgtt.ECom.DomainTest/Mgtt.ECom.DomainTest.csproj
dotnet sln Mgtt.ECom.sln add ./test/Mgtt.ECom.InfrastructureTest/Mgtt.ECom.InfrastructureTest.csproj
dotnet sln Mgtt.ECom.sln add ./test/Mgtt.ECom.PersistenceTest/Mgtt.ECom.PersistenceTest.csproj
```

### Compiling C# source code 

Run in order to compile C# source files in the `src` folder:

```sh
make build
```

### Running xUnit tests

Run in order to compile and run xUnit tests:

```sh
# All tests
make test
# Individual tests
make test-individual subdir=<subdirectory in the test folder, e.g. Mgtt.ECom.ApplicationTest>
```

### Starting the Kestrel-Webserver

Launch Docker Compose cluster and Kestrel-Webserver:

```sh
docker-compose up -d --build 
make run
```

### Generating project documentation

Run:

```sh
make docs
```
### Auto-format and lint C# files

Run:

```sh
make format-and-lint
```

**NOTE:** Optionally it is recommended to set up a symbolic link via `cd .git/hooks && ln -s ../../devops/scripts/format_and_lint.sh pre-commit && sudo chmod +x pre-commit && cd -` and a validation automation workflow to ensure that the `format_and_lint.sh` script is executed with each commit.

### Clearing artifacts

Run:

```sh
make clean
```

## Author

Marvin Gajek