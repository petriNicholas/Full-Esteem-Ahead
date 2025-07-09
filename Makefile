# Makefile for C# project using dotnet format and test

# Format code with detailed output
format:
	dotnet format Full-esteem-ahead.sln --verbosity diagnostic

# Fix formatting (alias for format)
fix: format

# Check if code is properly formatted (CI-safe, no changes made)
check:
	dotnet format Full-esteem-ahead.sln --verify-no-changes

# Run linter (alias for check)
lint: check

# Run tests (assuming test project exists)gi
test:
	dotnet test

# Restore dependencies
restore:
	dotnet restore

# Build project
build:
	dotnet build
