using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using WebDriverXUnit.Domain;
using WebDriverXUnit.Fixtures;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.Helpers.Interfaces;
using Xunit.Abstractions;

namespace WebDriverXUnit.Tests;

public class CreateMeeting : IClassFixture<GridUri> {
    private readonly GridUri _fixture;
    private ITestOutputHelper _testOutputHelper;
    private readonly ITestUsers _testUsers;
    private readonly UserCredentials _testUserCredentials;

    public CreateMeeting(GridUri fixture, ITestOutputHelper testOutputHelper)
    {
        _fixture = fixture;
        _testOutputHelper = testOutputHelper;

        string? environment = Environment.GetEnvironmentVariable("RUNTIME_ENVIRONMENT");

        if (string.IsNullOrEmpty(environment) || environment == "DEV") {
            _testUsers = new TestUsersFile();
        } else {
            _testUsers = new TestUsersEnvironment();
        }

        _testUserCredentials = _testUsers.GetTestUser();
    }

    [Fact]
    public async Task CreateMeeting() {
        DriverOptions[] driverOptions = Helpers.AvailableDriverOptions.Get();
        Task[] parallelTests = new Task[driverOptions.Length];

        for (int i = 0; i < Helpers.AvailableDriverOptions.GetAmount(); i++)
        {
            DriverOptions options = driverOptions[i];
            Task task = Task.Run(() => {
                RemoteWebDriver? driver = null;
                Uri? uri = _fixture.WebDriverUri;
                try
                {
                    // Login

                    // Create Meeting
                }
                catch (Exception e)
                {
                    ExceptionLogger.LogException(e, ref _testOutputHelper);
                }
                finally
                {
                    // Clean up in database after test

                    driver?.Quit();
                }
            });
        }
    }
}