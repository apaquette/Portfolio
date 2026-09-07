#!/usr/bin/env bash

set -u

targetdir="docs/reports/coverage-report"
resultsdir="artifacts/test-results"

# Remove all generated inputs and outputs from previous runs.
rm -rf "$resultsdir" "$targetdir"

mkdir -p "$resultsdir" "$targetdir"

if dotnet test Portfolio.sln \
    --results-directory "$resultsdir" \
    --collect:"XPlat Code Coverage;Format=opencover" \
    --settings coverage.runsettings
then
    echo "Tests ran successfully. Generating coverage report..."
else
    echo "Tests failed. Coverage report generation aborted."
    exit 1
fi

mapfile -d '' -t report_files < <(
    find "$resultsdir" \
        -type f \
        -name "coverage.opencover.xml" \
        -print0
)

if [ "${#report_files[@]}" -eq 0 ]; then
    echo "No OpenCover coverage files were found."
    exit 1
fi

reports=$(IFS=';'; printf '%s' "${report_files[*]}")

if reportgenerator \
    "-reports:$reports" \
    "-targetdir:$targetdir" \
    "-reporttypes:Html;SvgChart"
then
    echo "Coverage report generated successfully in '$targetdir'."
else
    echo "Failed to generate the coverage report."
    exit 1
fi
