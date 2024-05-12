using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using WebDriverXUnit.ClassData;
using WebDriverXUnit.Domain;
using WebDriverXUnit.Fixtures;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.Helpers.Interfaces;
using WebDriverXUnit.WindowDrivers;
using WebDriverXUnit.WindowDrivers.Interfaces;
using Xunit.Abstractions;

namespace WebDriverXUnit.Tests;

public class Authentication : IClassFixture<GridUri>
{
    private readonly GridUri _fixture;
    private ITestOutputHelper _testOutputHelper;
    private readonly UserCredentials _testUserCredentials;
    private readonly Uri _targetUri;

    public Authentication(GridUri fixture, ITestOutputHelper testOutputHelper)
    {
        _fixture = fixture;
        _testOutputHelper = testOutputHelper;

        string? testUuid = Environment.GetEnvironmentVariable("TEST_UUID")
            ?? throw new NullReferenceException("No value for environment variable 'TEST_UUID'");

        _testUserCredentials = new UserCredentials(testUuid + "@mail.dk", testUuid);

        string? targetUri = Environment.GetEnvironmentVariable("TARGET_URI")
            ?? throw new NullReferenceException("No value for environment variable 'TARGET_URI'");

        _targetUri = new Uri(targetUri);
    }

    [Fact(Skip = "Obselete")]
    public void FirefoxSmokeTest()
    {
        var options = new FirefoxOptions();
        var uri = _fixture.WebDriverUri;

        RemoteWebDriver? driver = null;
        try
        {
            driver = new RemoteWebDriver(uri, options);

            driver.Navigate().GoToUrl(_targetUri + "#/login");

            Assert.Equal("BetterBoard - Board Management System", driver.Title);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            IWebElement? header = wait.Until(webDriver => {
                 var element = webDriver.FindElement(By.TagName("h4"));
                 return element.Displayed ? element : null;
              });
             Assert.NotNull(header);
             Assert.Equal("Welcome to BetterBoard.", header.Text);

             _testOutputHelper.WriteLine("Smoketest complete");    
        }
        catch (Exception ex)
        {
            ExceptionLogger.LogException(ex, ref _testOutputHelper);
        }
        finally
        {
            driver?.Quit();
        }
        
    }

    [Fact(Skip = "Obselete")]
    public async Task SmokeTest()
    {
        DriverOptions[] driverOptions = Helpers.AvailableDriverOptions.Get();
        Task[] parallelTests = new Task[driverOptions.Length];

        for (int i = 0; i < Helpers.AvailableDriverOptions.GetAmount(); i++)
        {
            DriverOptions options = driverOptions[i];
            Task task = Task.Run(() => {
                RemoteWebDriver? driver = null;
                var uri = _fixture.WebDriverUri;
                try
                {
                    driver = new RemoteWebDriver(uri, options);

                    driver.Navigate().GoToUrl("https://dev.betterboard.dk/#/login");

                    Assert.Equal("BetterBoard - Board Management System", driver.Title);

                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                    wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
                    IWebElement? header = wait.Until(webDriver => {
                        var element = webDriver.FindElement(By.TagName("h4"));
                        return element.Displayed ? element : null;
                    });
                    Assert.NotNull(header);
                    Assert.Equal("Welcome to BetterBoard.", header.Text);

                    _testOutputHelper.WriteLine($"{options.BrowserName} WebDriver finished tests without errors");
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }
                finally
                {
                    driver?.Quit();
                }
            });
            parallelTests[i] = task;
        }

        await Task.WhenAll(parallelTests);

        _testOutputHelper.WriteLine("All WebDriver tests finished without errors in parallel");
    }

    [Fact]
    public async Task LoginTest() {
        DriverOptions[] driverOptions = Helpers.AvailableDriverOptions.Get();
        Task[] parallelTests = new Task[driverOptions.Length];
        bool failed = false;

        for (int i = 0; i < Helpers.AvailableDriverOptions.GetAmount(); i++)
        {
            DriverOptions options = driverOptions[i];
            Task task = Task.Run(() => {
                RemoteWebDriver? driver = null;
                Uri? uri = _fixture.WebDriverUri;
                try
                {
                    driver = new RemoteWebDriver(uri, options);

                    ILoginWindow loginWindow = new LoginWindow(driver, _targetUri);

                    loginWindow.Navigate();
                    loginWindow.AssertNavigation();

                    loginWindow.Login(_testUserCredentials);
                    loginWindow.AssertLogin(_testUserCredentials);
                    
                    _testOutputHelper.WriteLine($"{options.BrowserName} WebDriver Successfully logged in");

                }
                catch (Exception e)
                {
                    ExceptionLogger.LogException(e, ref _testOutputHelper);
                    failed = true;
                }
                finally
                {
                    driver?.Quit();
                }
            });
            parallelTests[i] = task;
        }

        await Task.WhenAll(parallelTests);

        Assert.False(failed);

        _testOutputHelper.WriteLine($"{nameof(LoginTest)} completed.");
    }
}