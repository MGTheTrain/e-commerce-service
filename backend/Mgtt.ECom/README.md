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

### Running xUnit tests

If external services for storage or messaging are needed via Docker Compose modify the [docker-compose.yml](./devops/docker-compose/docker-compose.yml) and utilize the following line:

```sh
make start-docker-cmp
```

Run xUnit tests:

```sh
# All tests
make test
# Individual tests
make test-individual subdir=<subdirectory in the test folder, e.g. Mgtt.ECom.ApplicationTest>
```

### Running smoke tests

Run smoke tests:

```sh
pip install -r requirements.txt # install pip depdencies
export API_BASE_URL='https://localhost:7260/api/v1' # Substitute the value if it differs
# all
make smoke-test
# Individual tests
make smoke-test-individual test_file_name=<test file name, e.g. test_user_management_domain_smoker.py>
```

### Starting the Kestrel-Webserver

If external services for storage or messaging are needed via Docker Compose modify the [docker-compose.yml](./devops/docker-compose/docker-compose.yml) and utilize the following line:

```sh
make start-docker-cmp
```

Start Kestrel-Webserver:

```sh
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