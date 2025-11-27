#!/bin/bash

# Run tests and collect code coverage
dotnet test --collect:"XPlat Code Coverage"

# Check if tests ran successfully
if [ $? -eq 0 ]; then
    echo "Test ran successfully. Generating coverage report..."

    targetdir="docs/reports/coverage-report"
    historical_reports="docs/reports/historical-reports"

    # Create output dir if it doesn't exist
    mkdir -p $targetdir
    mkdir -p $historical_reports

    report_files=(
        "tests/Portfolio.Shared.Test/TestResults/coverage.opencover.xml"
    )

    reports=$(IFS=';'; echo "${report_files[*]}")


    # Run report generated with specified report fields
    reportgenerator -reports:$reports -targetdir:$targetdir -reporttypes:"Html;SvgChart" -historydir:$historical_reports

    if [ $? -eq 0 ]; then
        echo "Coverage report generated successfully in '$targetdir' directory."
    else
        echo "Failed to generate the coverage report."
    fi
else
    echo "Tests failed. Coverage report generation aborted."
fi