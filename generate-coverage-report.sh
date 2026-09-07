#!/usr/bin/env bash

set -u

if dotnet test Portfolio.sln \
    --collect:"XPlat Code Coverage;Format=opencover" \
    /p:ExcludeByFile="**/Program.cs"

then
    echo "Tests ran successfully. Generating coverage report..."

    targetdir="docs/reports/coverage-report"
    historical_reports="docs/reports/historical-reports"

    mkdir -p "$targetdir" "$historical_reports"

    mapfile -t report_files < <(
        find tests \
            -type f \
            -name "coverage.opencover.xml" \
            -print
    )

    if [ "${#report_files[@]}" -eq 0 ]; then
        echo "No OpenCover coverage files were found."
        exit 1
    fi

    reports=$(IFS=';'; printf '%s' "${report_files[*]}")

    if reportgenerator \
        "-reports:$reports" \
        "-targetdir:$targetdir" \
        "-reporttypes:Html;SvgChart" \
        "-historydir:$historical_reports"
    then
        echo "Coverage report generated successfully in '$targetdir'."
    else
        echo "Failed to generate the coverage report."
        exit 1
    fi
else
    echo "Tests failed. Coverage report generation aborted."
    exit 1
fi
