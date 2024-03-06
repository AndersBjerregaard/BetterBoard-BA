# Run Tests With Logger Output

Execute the test runner with the `logger` option:

```
dotnet test --logger "console;verbosity=detailed"
```

# Linting

[*SonarLint*](https://www.sonarsource.com/products/sonarlint/)

Install to your IDE.

It's also added to the project, to account for vulnerabilities and code smell in the CI / CD:
```
dotnet add package SonarAnalyzer.CSharp --version 9.21.0.86780
```