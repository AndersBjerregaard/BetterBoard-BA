FROM ubuntu:22.04

ARG ARCH="x64"
# Prevents prompts
ARG DEBIAN_FRONTEND=noninteractive
ARG RUNNER_VERSION="2.316.1"

RUN apt update -y && apt upgrade -y

RUN useradd -m docker

# Uuid generation package
RUN apt install uuid-runtime

# Packages for .net
RUN apt install -y --no-install-recommends \
    curl jq ca-certificates dotnet-sdk-8.0

RUN curl -sL https://aka.ms/InstallAzureCLIDeb | bash

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