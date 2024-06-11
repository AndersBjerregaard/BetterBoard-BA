using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using WebDriverXUnit.Domain;
using WebDriverXUnit.Extensions;
using WebDriverXUnit.Factories;
using WebDriverXUnit.Fixtures;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers;
using WebDriverXUnit.WindowDrivers.Interfaces;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace WebDriverXUnit.Tests;

public class LoginTests(ITestOutputHelper testOutputHelper)
{
    private ITestOutputHelper _testOutputHelper = testOutputHelper;
    private readonly UserCredentials _testUserCredentials = TestVariablesFactory.GetUserCredentials();
    private readonly Uri _targetUri = TestVariablesFactory.GetSutUri();
    private readonly Uri _gridUri = TestVariablesFactory.GetSeleniumGridUri();
    private readonly string[] _defaultWebDriverArguments = TestVariablesFactory.DEFAULT_WEBDRIVER_ARGUMENTS;

    [Fact(Skip = "Obselete")]
    public void FirefoxSmokeTest()
    {
        var options = new FirefoxOptions();
        options.ApplyOptionsArguments(_defaultWebDriverArguments);

        RemoteWebDriver? driver = null;
        try
        {
            driver = new RemoteWebDriver(_gridUri, options);

            driver.Navigate().GoToUrl(_targetUri + "#/login");

            Assert.Equal("BetterBoard - Board Management System", driver.Title);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            IWebElement? header = wait.Until(webDriver =>
            {
                var element = webDriver.FindElement(By.TagName("h4"));
                return element.Displayed ? element : null;
            });
            Assert.NotNull(header);
            Assert.Equal("Welcome to BetterBoard.", header.Text);

            _testOutputHelper.WriteLine("Smoketest complete");
        }
        catch (Exception e)
        {
            ExceptionLogger.LogException(e, ref _testOutputHelper, options.BrowserName);
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
                var uri = _gridUri;
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
        DriverOptions[] driverOptions = AvailableDriverOptions.Get();
        Task[] parallelTests = new Task[driverOptions.Length];
        bool failed = false;

        for (int i = 0; i < driverOptions.Length; i++)
        {
            DriverOptions options = driverOptions[i];
            options.ApplyOptionsArguments(_defaultWebDriverArguments);
            Task task = Task.Run(() =>
            {
                RemoteWebDriver? driver = null;
                try
                {
                    driver = new RemoteWebDriver(_gridUri, options);

                    ILoginWindow loginWindow = new LoginWindow(driver, _targetUri);
                    loginWindow.Navigate();
                    loginWindow.AssertNavigation();
                    loginWindow.Login(_testUserCredentials);
                    loginWindow.AssertLogin(_testUserCredentials);
                    
                    _testOutputHelper.WriteLine($"[SUCCESS] {options.BrowserName} WebDriver successfully logged in");

                }
                catch (Exception e)
                {
                    ExceptionLogger.LogException(e, ref _testOutputHelper, options.BrowserName);
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
    }

}