# Variables
DOTNET = dotnet
PYTEST = pytest

# Directories
DOCS_DIR = docs
SRC_DIR = src
TEST_DIR = test
SMOKE_TEST_DIR = smoke-test

# Commands
.PHONY: start-docker-cmp-external stop-docker-cmp-external test test-individual smoke-test run clean format-and-lint

start-docker-cmp-external:
	cd ../.. && docker-compose -f docker-compose.external.yml up -d --build

stop-docker-cmp-external:
	cd ../.. && docker-compose -f docker-compose.external.yml down -v

# Run tests
test:
	@folders=$$(find ./test -type f -name '*.csproj' -exec dirname {} \;); \
	for folder in $${folders}; do \
		echo "Running tests in folder: $${folder}"; \
		dotnet test "$${folder}"; \
	done

test-individual:
	@if [ -z "$(subdir)" ]; then \
		echo "Please provide a subdirectory in the test folder using 'make test-individual subdir=<subdirectory>'"; \
	else \
		$(DOTNET) test $(TEST_DIR)/$(subdir); \
	fi

smoke-test:
	cd smoke-test && $(PYTEST) --html=report.html

smoke-test-individual:
	@if [ -z "$(test_file_name)" ]; then \
		echo "Please provide a testname in the test folder using 'make smoke-test-individual test_file_name=<test file name, e.g. test_user_management_domain_smoker.py>'"; \
	else \
		cd smoke-test && $(PYTEST) $(test_file_name) --html=report.html; \
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

