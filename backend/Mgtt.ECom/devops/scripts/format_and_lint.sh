#!/bin/bash

set -euo pipefail

SCRIPT_DIR=$(dirname "$BASH_SOURCE")
ROOT_PROJECT_DIR=$SCRIPT_DIR/../..

cd $ROOT_PROJECT_DIR

BLUE='\033[0;34m'
NC='\033[0m'

echo "#####################################################################################################"
echo -e "$BLUE INFO: $NC About to apply linting on and auto-format C# files"

for folder in $(find ./** -name 'stylecop.json' -exec dirname {} \;); do
	cd $folder
	echo "#####################################################################################################"
	echo -e "$BLUE INFO: $NC About to lint C# files in $folder"
	dotnet restore *.csproj
	dotnet format
	cd -
done

# yml formatter
# jekyll formatter

cd -