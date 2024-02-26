# Sourcecode Structure

`Selenium-Docker` contains all the infrastructure needed to configure a fully distributed Selenium Grid experience. Configured from the standpoint of Docker. For the sake of being reusable in a local development environment, as well as a production environment, hosted by a cloud provider. Files include:
- Configuration files using `toml` syntax.
- Dockerfiles
- Docker Compose files in `yaml` syntax.
- `Shell` scripts to help automate tasks and communication

`WebDriverTests` contains the sourcecode for the main test suite. It acts as documentation and execution point for all E2E tests. Written as an [xunit](https://xunit.net/) test project, it contains various *userstories* written in code against the [Selenium.WebDriver](https://www.nuget.org/packages/Selenium.WebDriver) API.