#!/bin/bash

# Script để chạy tests cho InteractHub project

echo "========================================="
echo "  INTERACTHUB - TEST RUNNER"
echo "========================================="
echo ""

# Colors
GREEN='\033[0;32m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# 1. Build project
echo -e "${BLUE}[1/3] Building test project...${NC}"
dotnet build InteractHub.Tests/InteractHub.Tests.csproj --configuration Release

if [ $? -ne 0 ]; then
    echo "Build failed!"
    exit 1
fi

echo ""

# 2. Run tests
echo -e "${BLUE}[2/3] Running tests...${NC}"
dotnet test InteractHub.Tests/InteractHub.Tests.csproj --configuration Release --logger "console;verbosity=normal"

if [ $? -ne 0 ]; then
    echo "Tests failed!"
    exit 1
fi

echo ""

# 2. Run tests with coverage and generate HTML report
echo -e "${BLUE}[2/3] Running tests and collecting coverage...${NC}"

# Ensure ReportGenerator (dotnet-reportgenerator-globaltool) is available
if ! command -v reportgenerator &> /dev/null; then
    echo -e "${BLUE}ReportGenerator not found. Installing global tool...${NC}"
    dotnet tool install -g dotnet-reportgenerator-globaltool --version 5.1.10
    if [ $? -ne 0 ]; then
        echo "Failed to install reportgenerator. You can install it manually:"
        echo "  dotnet tool install -g dotnet-reportgenerator-globaltool"
        exit 1
    fi
fi

# Run tests and collect coverage using the Coverlet collector
TEST_RESULTS_DIR="TestResults"
COVERAGE_OUTPUT_DIR="coverage-results"
rm -rf "$COVERAGE_OUTPUT_DIR"
mkdir -p "$COVERAGE_OUTPUT_DIR"

dotnet test InteractHub.Tests/InteractHub.Tests.csproj --configuration Release --logger "console;verbosity=normal" --collect:"XPlat Code Coverage"
if [ $? -ne 0 ]; then
    echo "Tests failed!"
    exit 1
fi

# Find the generated coverage file (cobertura) and generate HTML report
coverage_file=$(find $TEST_RESULTS_DIR -type f -name "coverage.cobertura.xml" | tail -n 1)
if [ -z "$coverage_file" ]; then
    echo "Coverage file not found. Make sure Coverlet collector is referenced in the test project." 
    exit 0
fi

echo -e "${BLUE}Generating HTML coverage report...${NC}"
reportgenerator -reports:"$coverage_file" -targetdir:"$COVERAGE_OUTPUT_DIR" -reporttypes:Html
if [ $? -ne 0 ]; then
    echo "ReportGenerator failed to create HTML report." 
    exit 1
fi

# 3. Summary
echo -e "${GREEN}=========================================${NC}"
echo -e "${GREEN}  TEST RUN & COVERAGE REPORT COMPLETE${NC}"
echo -e "${GREEN}=========================================${NC}"
echo ""
echo "HTML coverage report generated at: $COVERAGE_OUTPUT_DIR/index.htm"
echo "For detailed test cases, see: InteractHub.Tests/TEST_CASES.md"
