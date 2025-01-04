# Variables
DOTNET = dotnet
PYTEST = pytest

# Directories
DOCS_DIR = docs
SRC_DIR = src
TEST_DIR = test
SMOKE_TEST_DIR = api-test

# Commands
.PHONY: start-docker-cmp-external stop-docker-cmp-external run-unit-tests run-integration-tests run-selected-tests run-api-test run-selected-api-test run clean format-and-lint

help:
	@echo "Available make commands:"
	@echo "  start-docker-cmp-external  - Start the Docker Compose services"
	@echo "  stop-docker-cmp-external   - Stop the Docker Compose services"
	@echo "  run-unit-tests             - Run all unit tests"
	@echo "  run-integration-tests      - Run all integration tests"
	@echo "  run-selected-tests         - Run an selected tests (provide subdir=<subdirectory>)"
	@echo "  run-api-tests         		- Run api tests"
	@echo "  run-selected-api-test      - Run selected api test"
	@echo "  run                        - Start the web server"
	@echo "  clean                      - Clean up generated files"
	@echo "  docs                       - Build and serve the documentation"
	@echo "  format-and-lint            - Format and lint C# files"
	@echo "  help                       - Display this help message"

start-docker-cmp-external:
	cd ../.. && docker-compose -f docker-compose.external.yml up -d --build

stop-docker-cmp-external:
	cd ../.. && docker-compose -f docker-compose.external.yml down -v

# Run tests
run-unit-tests:
	@folders=$$(find ./test/Unit -type f -name '*.csproj' -exec dirname {} \;); \
	for folder in $${folders}; do \
		echo "Running tests in folder: $${folder}"; \
		dotnet test "$${folder}"; \
	done

run-integration-tests:
	@folders=$$(find ./test/Integration -type f -name '*.csproj' -exec dirname {} \;); \
	for folder in $${folders}; do \
		echo "Running tests in folder: $${folder}"; \
		dotnet test "$${folder}"; \
	done

run-selected-tests:
	@if [ -z "$(subdir)" ]; then \
		echo "Please provide a subdirectory in the test folder using 'make run-selected-tests subdir=<subdirectory>'"; \
	else \
		$(DOTNET) test $(TEST_DIR)/$(subdir); \
	fi

run-api-tests:
	cd test/Api && $(PYTEST) --html=report.html

run-selected-api-test:
	@if [ -z "$(test_file_name)" ]; then \
		echo "Please provide a testname in the test folder using 'make run-selected-api-test test_file_name=<test file name, e.g. test_shopping_cart_domain_smoker.py>'"; \
	else \
		cd test/Api && $(PYTEST) $(test_file_name) --html=report.html; \
	fi

# Start server
run:
	cd src/Mgtt.ECom.Web && dotnet run

# Clean up generated files
clean:
	rm -rf $(DOCS_DIR)/_build
	rm -rf ${DOCS_DIR}/Mgtt.ECom/_site
	rm -rf ${DOCS_DIR}/Mgtt.ECom/.jekyll-cache
	rm -rf ${SMOKE_TEST_DIR}/__pycache__
	rm -rf ${SMOKE_TEST_DIR}/assets
	rm -rf ${SMOKE_TEST_DIR}/report.html
	find $(SRC_DIR) $(TEST_DIR) -type d \( -name 'obj' -o -name 'bin' \) -exec rm -rf {} +

# Format and lint C# files
format-and-lint:
	find . -type f -name '*.csproj' -execdir dotnet format {} \;

