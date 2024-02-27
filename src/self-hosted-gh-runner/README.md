# Self-hosted GitHub actions runner in docker

[*source article*](https://baccini-al.medium.com/how-to-containerize-a-github-actions-self-hosted-runner-5994cc08b9fb)

**Note:** The `start.sh` script relies on the parameters `REPO` and `TOKEN` being bassed to it.

## Building image

```
docker build --tag <your-tag> --file .Dockerfile .
```

## Running

Populate environment variables `REPO` and `TOKEN`

## TODO:

runner-1  | Http response code: NotFound from 'POST https://api.github.com/actions/runner-registration' (Request Id: BAC3:2A4E86:15F22FB:1622CBC:65DDF59C)
runner-1  | {"message":"Not Found","documentation_url":"https://docs.github.com/rest"}
runner-1  | Response status code does not indicate success: 404 (Not Found).