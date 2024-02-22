FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.19

RUN apk update

RUN apk add jq

RUN mkdir /app

WORKDIR /app

COPY . ./

RUN chmod +x wait_for_grid.sh entrypoint.sh

RUN dotnet restore --verbosity detailed

RUN dotnet build --no-restore

ENTRYPOINT [ "/app/entrypoint.sh" ]