# Self-hosted GitHub actions runner in docker

[*source article*](https://baccini-al.medium.com/how-to-containerize-a-github-actions-self-hosted-runner-5994cc08b9fb)

**Note:** The `start.sh` script relies on the parameters `REPO` and `TOKEN` being bassed to it.

## Building image

```
docker build --tag <your-tag> --file .Dockerfile .
```

## Prerequisite

Populate environment variables `REPO` and `TOKEN`

Example `.env` file:
```env
# .env
REPO=foo/bar
ACCESS_TOKEN=1234
```

## Running with `docker run`

```shell
docker run -dti --network=host --env-file .env --name gh-actions-runner anders97/gh-actions-runner:latest
```

## Create a registration token for a repository

[*source article*](https://docs.github.com/en/rest/actions/self-hosted-runners?apiVersion=2022-11-28#create-a-registration-token-for-a-repository)

This is used for a self-hosted runner, in order to authenticate it.
The endpoint works with the following token types:
- [GitHub App user access tokens](https://docs.github.com/en/apps/creating-github-apps/authenticating-with-a-github-app/generating-a-user-access-token-for-a-github-app)
- [GitHub App installation access tokens](https://docs.github.com/en/apps/creating-github-apps/authenticating-with-a-github-app/generating-an-installation-access-token-for-a-github-app)
- [Fine-grained personal access tokens](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens#creating-a-fine-grained-personal-access-token)

The token must have the following permission set:
- `administration:write`

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

# TODO:

Add relevant capabilities to runner