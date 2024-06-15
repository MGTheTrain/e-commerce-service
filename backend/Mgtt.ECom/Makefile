# Variables
DOTNET = dotnet

# Directories
DOCS_DIR = docs
SRC_DIR = src
TEST_DIR = test

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
		echo "Please provide a subdirectory in the test folder using 'make test subdir=<subdirectory>'"; \
	else \
		$(DOTNET) test $(TEST_DIR)/$(subdir); \
	fi

smoke-test:
	cd smoke-test && pytest --html=report.html

# Start server
run:
	cd src/Mgtt.ECom.Web && dotnet run

# Clean up generated files
clean:
	rm -rf $(DOCS_DIR)/_build
	rm -rf ${DOCS_DIR}/Mgtt.ECom/_site
	rm -rf ${DOCS_DIR}/Mgtt.ECom/.jekyll-cache
	find $(SRC_DIR) $(TEST_DIR) -type d \( -name 'obj' -o -name 'bin' \) -exec rm -rf {} +

# Build and serve documentation
docs:
	cd docs/Mgtt.ECom && jekyll serve && echo "Visit in a browser of choice localhost:4000"

# Format and lint C# files
format-and-lint:
	cd devops/scripts && ./format_and_lint.sh

