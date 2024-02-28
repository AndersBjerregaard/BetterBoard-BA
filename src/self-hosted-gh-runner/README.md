# Self-hosted GitHub actions runner in docker

[*source article*](https://baccini-al.medium.com/how-to-containerize-a-github-actions-self-hosted-runner-5994cc08b9fb)

**Note:** The `start.sh` script relies on the parameters `REPO` and `TOKEN` being bassed to it.

## Building image

```
docker build --tag <your-tag> --file .Dockerfile .
```

## Running

Populate environment variables `REPO` and `TOKEN`

## Create a registration token for a repository

[*source article*](https://docs.github.com/en/rest/actions/self-hosted-runners?apiVersion=2022-11-28#create-a-registration-token-for-a-repository)

This is used for a self-hosted runner, in order to authenticate it.

Example request:
*`OWNER` and `REPO` are not case sensitive.*
```shell
curl -L \
  -X POST \
  -H "Accept: application/vnd.github+json" \
  -H "Authorization: Bearer <YOUR-TOKEN>" \
  -H "X-GitHub-Api-Version: 2022-11-28" \
  https://api.github.com/repos/OWNER/REPO/actions/runners/registration-token
```

Example response:
```json
{
  "token": "LLBF3JGZDX3P5PMEXLND6TS6FCWO6",
  "expires_at": "2020-01-22T12:13:35.123-08:00"
}
```

## TODO:

runner-1  | Http response code: NotFound from 'POST https://api.github.com/actions/runner-registration' (Request Id: BAC3:2A4E86:15F22FB:1622CBC:65DDF59C)
runner-1  | {"message":"Not Found","documentation_url":"https://docs.github.com/rest"}
runner-1  | Response status code does not indicate success: 404 (Not Found).