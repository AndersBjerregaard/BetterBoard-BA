#!/bin/bash

REPO=$REPO
ACCESS_TOKEN=$ACCESS_TOKEN
RUNNER_NAME="self-hosted-$RANDOM"

# echo "curl -L -X POST -H \"Accept: application/vnd.github+json\" -H \"Authorization: Bearer ${ACCESS_TOKEN}\" -H \"X-GitHub-Api-Version: 2022-11-28\" https://api.github.com/repos/${REPO}/actions/runners/registration-token"
REG_TOKEN=$(curl -L -X POST -H "Accept: application/vnd.github+json" -H "Authorization: Bearer ${ACCESS_TOKEN}" -H "X-GitHub-Api-Version: 2022-11-28" https://api.github.com/repos/${REPO}/actions/runners/registration-token | jq .token --raw-output)
echo ${REG_TOKEN}

cd /home/docker/actions-runner

./config.sh --url https://github.com/${REPO} --token ${REG_TOKEN} --name ${RUNNER_NAME}  --unattended

cleanup() {
    echo "Removing runner..."
    ./config.sh remove --token ${REG_TOKEN}
}

trap 'cleanup; exit 130' INT
trap 'cleanup; exit 143' TERM

./run.sh & wait $!