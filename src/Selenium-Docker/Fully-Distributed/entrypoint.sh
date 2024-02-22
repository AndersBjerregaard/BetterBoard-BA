#!/bin/bash
# entrypoint.sh

/app/wait-for-grid.sh

# Check the exit code of script above
if [ $? -eq 0 ]; then
  exec dotnet test
else
  exit $?
fi