#!/bin/bash
# wait_for_grid.sh

set -e
url="default"

while getopts "u:" opt; do
    case ${opt} in
        u )
            url="$OPTARG"
            ;;
        \? )
            echo "Usage: $0 [-u VALUE]"
            exit 1
            ;;
    esac
done

shift $((OPTIND -1))

if [ "$url" = "default" ] ; then
    echo "Selenium uri was unset. Use -u flag with a uri when executing."
    echo "Example:"
    echo "./wait_for_grid.sh -u http://localhost/status:4444"
    exit 1
fi

wait_interval_in_seconds=1
max_wait_time_in_seconds=30
end_time=$((SECONDS + max_wait_time_in_seconds))
time_left=$max_wait_time_in_seconds

while [ $SECONDS -lt $end_time ]; do
    response=$(curl -sL "$url" | jq -r '.value.ready')
    if [ -n "$response"  ]  && [ "$response" ]; then
        echo "Selenium Grid is up - executing tests"
        exit 0
    else
        echo "Waiting for the Grid. Sleeping for $wait_interval_in_seconds second(s). $time_left seconds left until timeout."
        sleep $wait_interval_in_seconds
        time_left=$((time_left - wait_interval_in_seconds))
    fi
done

if [ $SECONDS -ge $end_time ]; then
    echo "Timeout: The Grid was not started within $max_wait_time_in_seconds seconds."
    exit 1
fi
