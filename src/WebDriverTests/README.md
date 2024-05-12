# Run Tests With Logger Output

Execute the test runner with the `logger` option:

```
dotnet test --logger "console;verbosity=detailed"
```

Optionally, pass an environment variable value to set the Selenium WebDriver uri:

```
dotnet test --logger "console;verbosity=detailed" -e GRID_URI="http://localhost:8082" --filter WebDriverXUnit.Tests.Authentication.FirefoxSmokeTest
```

# Proper Execution

Pass env variables for `GRID_URI`, `TEST_UUID`, and `TARGET_URI`.

```
dotnet test --logger "console;verbosity=detailed" -e GRID_URI="" -e TARGET_URI="https://dev.betterboard.dk/" -e TEST_UUID="" --filter LoginTest
```

# Linting

[*SonarLint*](https://www.sonarsource.com/products/sonarlint/)

Install to your IDE.

It's also added to the project, to account for vulnerabilities and code smell in the CI / CD:
```
dotnet add package SonarAnalyzer.CSharp --version 9.21.0.86780
```