# Variables
DOTNET = dotnet
PYTEST = pytest

# Directories
DOCS_DIR = docs
SRC_DIR = src
TEST_DIR = test
SMOKE_TEST_DIR = smoke-test

# Commands
.PHONY: start-docker-cmp stop-docker-cmp test test-individual smoke-test run clean docs format-and-lint

start-docker-cmp:
	cd devops/docker-compose && docker-compose up -d --build

stop-docker-cmp:
	cd devops/docker-compose && docker-compose down -v

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
	cd smoke-test && pip install -r requirements.txt && $(PYTEST) --html=report.html

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
	rm ${SMOKE_TEST_DIR}/report.html
	find $(SRC_DIR) $(TEST_DIR) -type d \( -name 'obj' -o -name 'bin' \) -exec rm -rf {} +

# Build and serve documentation
docs:
	cd docs/Mgtt.ECom && jekyll serve && echo "Visit in a browser of choice localhost:4000"

# Format and lint C# files
format-and-lint:
	cd devops/scripts && ./format_and_lint.sh

