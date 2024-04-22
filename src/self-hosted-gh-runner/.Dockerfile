FROM ubuntu:22.04

ARG ARCH="x64"
# Prevents prompts
ARG DEBIAN_FRONTEND=noninteractive
ARG RUNNER_VERSION="2.313.0"

RUN apt update -y && apt upgrade -y

RUN useradd -m docker

# Packages for .net
RUN apt install -y --no-install-recommends \
    curl jq ca-certificates dotnet-sdk-8.0

RUN curl -sL https://aka.ms/InstallAzureCLIDeb | bash

# RUN apt-get update

# # Packages for azure cli
# RUN apt-get install -y --no-install-recommends \
#     apt-transport-https gnupg lsb-release

# # Microsoft signing key
# RUN mkdir -p /etc/apt/keyrings
# RUN curl -sLS https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor -o /etc/apt/keyrings/microsoft.gpg
# RUN chmod go+r /etc/apt/keyrings/microsoft.gpg

# # Add azure cli software repo
# COPY azure-cli.sources /etc/apt/sources.list.d/azure-cli.sources

# RUN apt-get update

# # Install azure cli
# RUN apt-get install -y --no-install-recommends \
#     azure-cli=2.51.0-1~bullseye

# GH actions runner boilerplate
RUN mkdir -v /home/docker/actions-runner

WORKDIR /home/docker/actions-runner

RUN curl -o actions-runner-linux-x64-${RUNNER_VERSION}.tar.gz -L https://github.com/actions/runner/releases/download/v${RUNNER_VERSION}/actions-runner-linux-x64-${RUNNER_VERSION}.tar.gz

RUN tar xzf ./actions-runner-linux-x64-${RUNNER_VERSION}.tar.gz

RUN chown -R docker ~docker && /home/docker/actions-runner/bin/installdependencies.sh

COPY start.sh start.sh

RUN chmod +x start.sh

USER docker

ENTRYPOINT [ "./start.sh" ]