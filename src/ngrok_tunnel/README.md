# Automatic localhost tunnelling

Make sure to run an instance of ngrok, use a static domain, and point the target uri of the selenium grid from the webdriver (`./src/WebDriverTests/wait_for_grid.sh -u <NGROK_TUNNEL_DOMAIN>`).
If the purpose is to execute tests from a distributed sense - i.e. a self-hosted runner, executing a github actions workflow.