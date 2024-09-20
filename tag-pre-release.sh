#!/bin/bash

if [ $# -ne 1 ]; then
    echo "Usage: $0 <version>"
    exit 1
fi

version_regex="^v[0-9]+\.[0-9]+\.[0-9]+-rc\.[0-9]+$"

if [[ $1 =~ $version_regex ]]; then
    git tag "$0"
    git push origin tag "$0"
else
    echo "Invalid version format. Expected format: v[0-9]+.[0-9]+.[0-9]+-rc\.[0-9]+"
    exit 1
fi
