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
- [Paypal Order API](https://developer.paypal.com/docs/api/orders/v2/)

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

If external services for storage or messaging are needed via Docker Compose modify the [docker-compose.external.yml](../../docker-compose.external.yml) and utilize the following line:

```sh
make start-docker-cmp-external
```

Run unit tests:

```sh
make run-unit-test
```

Run integration tests:

```sh
export AWS_ACCESS_KEY_ID="test"
export AWS_SECRET_ACCESS_KEY="test"
export AWS_DEFAULT_REGION="us-east-1"
export AWS_ENDPOINT_URL="http://localhost:4566"

make run-integration-tests
```

Run selected xUnit tests:

```sh
make run-selected-tests subdir=<subdirectory in the test folder, e.g. Unit/Mgtt.ECom.DomainTest or Integration/Mgtt.ECom.ApplicationTest>
```

### Starting the Kestrel-Webserver

If external services for storage or messaging are needed via Docker Compose modify the [docker-compose.external.yml](../../docker-compose.external.yml) and utilize the following line:

```sh
make start-docker-cmp-external
```

Start Kestrel-Webserver:

```sh
make run
```

### Running api tests

Run api tests:

```sh
pip install -r requirements.txt # install pip depdencies
export API_BASE_URL='http://localhost:5000/api/v1' # Substitute the value if it differs
export BEARER_TOKEN='<your Auth0 access token>' # A user with claim permissions to manage reviews, orders, products, and carts. Retrieve the bearer token from browser storage via the web frontend
# all
make run-api-tests
# Individual tests
make run-selected-api-test test_file_name=<test file name, e.g. test_shopping_cart_domain_smoker.py>
```

Results should resemble the following (the **left half of the screen** is executing the smoke test while **the right half** is ramping up the Kestrel web server as a precondition):

![api tests results](../../docs/results/api-tests-results.PNG)

After running the tests a **test report is generated** and available for review. To view it, open the **generated file test/Api/report.html** in your preferred web browser:

![api tests report](../../docs/results/api-tests-report.PNG)

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