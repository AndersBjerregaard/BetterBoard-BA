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
using Xunit.Abstractions;

namespace WebDriverXUnit.Tests;

public class Authentication : IClassFixture<GridUri>
{
    private readonly GridUri _fixture;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ITestUsers _testUsers;
    private readonly UserCredentials _testUserCredentials;

    public Authentication(GridUri fixture, ITestOutputHelper testOutputHelper)
    {
        _fixture = fixture;
        _testOutputHelper = testOutputHelper;
        _testUsers = new TestUsersEnvironment();
        _testUserCredentials = _testUsers.GetTestUser();
    }

    [Fact]
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

        for (int i = 0; i < Helpers.AvailableDriverOptions.GetAmount(); i++)
        {
            DriverOptions options = driverOptions[i];
            Task task = Task.Run(() => {
                RemoteWebDriver? driver = null;
                Uri? uri = _fixture.WebDriverUri;
                try
                {
                    driver = new RemoteWebDriver(uri, options);

                    driver.Navigate().GoToUrl("https://dev.betterboard.dk/#/login");

                    Assert.Equal("BetterBoard - Board Management System", driver.Title);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                    wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
                    IWebElement? header = wait.Until<IWebElement?>(webDriver => {
                        IWebElement element = webDriver.FindElement(By.TagName("h4"));
                        return element.Displayed ? element : null;
                    });
                    Assert.NotNull(header);
                    Assert.Equal("Welcome to BetterBoard.", header.Text);

                    // Find fields
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5)) { PollingInterval = TimeSpan.FromMilliseconds(500)};
                    wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException), typeof(ElementNotInteractableException));
                    var element = driver.FindElement(By.ClassName("form-control"));
                    bool fieldDisplayed = wait.Until(x => element.Displayed);
                    Assert.True(fieldDisplayed);

                    var fields = driver.FindElements(By.ClassName("form-control"));
                    Assert.NotEmpty(fields);
                    var submit = driver.FindElement(By.ClassName("btn-primary"));
                    Assert.NotNull(submit);

                    var usernameField = fields[0];
                    usernameField.SendKeys(_testUserCredentials.Username);
                    var passwordField = fields[1];
                    passwordField.SendKeys(_testUserCredentials.Password);

                    submit.Click();

                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)) { PollingInterval = TimeSpan.FromMilliseconds(500)};
                    wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException), typeof(ElementNotInteractableException), typeof(NoSuchElementException));
                    var boardHeader = wait.Until(x => {
                        var element = x.FindElement(By.TagName("h2"));
                        return element.Displayed ? element : null;
                    });
                    
                    Assert.Contains("John Test", boardHeader?.Text);

                    _testOutputHelper.WriteLine($"{options.BrowserName} WebDriver Successfully logged in");

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
    public void EnvironmentTest() {

        _testOutputHelper.WriteLine($"Email: {_testUserCredentials.Username}\nPassword: {_testUserCredentials.Password}");
    }
}