FROM ubuntu:22.04

ARG ARCH="x64"
# Prevents prompts
ARG DEBIAN_FRONTEND=noninteractive
ARG RUNNER_VERSION="2.313.0"

RUN apt update -y && apt upgrade -y

RUN useradd -m docker

RUN apt install -y --no-install-recommends \
    curl jq ca-certificates

# Prep dotnet installation
RUN mkdir -vp /usr/share/dotnet

RUN chmod go=w /usr/share/dotnet

RUN mkdir -v /home/docker/actions-runner

WORKDIR /home/docker/actions-runner

RUN curl -o actions-runner-linux-x64-${RUNNER_VERSION}.tar.gz -L https://github.com/actions/runner/releases/download/v${RUNNER_VERSION}/actions-runner-linux-x64-${RUNNER_VERSION}.tar.gz

RUN tar xzf ./actions-runner-linux-x64-${RUNNER_VERSION}.tar.gz

RUN chown -R docker ~docker && /home/docker/actions-runner/bin/installdependencies.sh

COPY start.sh start.sh

RUN chmod +x start.sh

USER docker

ENTRYPOINT [ "./start.sh" ]