using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
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

public class WebDriverTests : IClassFixture<TestVariables>
{
    private readonly TestVariables _fixture;
    private ITestOutputHelper _testOutputHelper;
    private readonly UserCredentials _testUserCredentials;
    private readonly Uri _targetUri;
    private readonly Uri _gridUri;
    private static readonly string[] DEFAULT_WEBDRIVER_ARGUMENTS = ["--no-sandbox", "--disable-dev-shm-usage", "--incognito"];

    public WebDriverTests(TestVariables fixture, ITestOutputHelper testOutputHelper)
    {
        _fixture = fixture;
        _testOutputHelper = testOutputHelper;

        _testUserCredentials = _fixture.TestUuid;

        _targetUri = _fixture.TargetUri;

        _gridUri = _fixture.WebDriverUri;
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
        DriverOptions[] driverOptions = [Helpers.AvailableDriverOptions.EDGE_OPTIONS];
        Task[] parallelTests = new Task[driverOptions.Length];
        bool failed = false;

        for (int i = 0; i < driverOptions.Length; i++)
        {
            DriverOptions options = driverOptions[i];
            ModifyDriverOptions(options);
            Task task = Task.Run(() =>
            {
                RemoteWebDriver? driver = null;
                try
                {
                    driver = new RemoteWebDriver(_gridUri, options);

                    _ = LoginSession.Login(driver, _targetUri, _testUserCredentials);

                    _testOutputHelper.WriteLine($"[SUCCESS] {options.BrowserName} WebDriver successfully logged in");

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
    }

    [Fact(Skip = "Not Implemented")]
    public async Task FrontPageTest() {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task MeetingCreationTest() {
        DriverOptions[] driverOptions = [Helpers.AvailableDriverOptions.EDGE_OPTIONS];
        Task[] parallelTests = new Task[driverOptions.Length];
        bool failed = false;

        for (int i = 0; i < driverOptions.Length; i++)
        {
            DriverOptions options = driverOptions[i];
            ModifyDriverOptions(options);
            Task task = Task.Run(() =>
            {
                RemoteWebDriver? driver = null;
                try
                {
                    driver = new RemoteWebDriver(_gridUri, options);

                    _ = LoginSession.Login(driver, _targetUri, _testUserCredentials);

                    IBoardsWindow boardsWindow = new BoardsWindow(driver, _targetUri);

                    boardsWindow.Navigate();
                    boardsWindow.AssertNavigation();

                    boardsWindow.GoToBoard("Test Board", ref _testOutputHelper);
                    boardsWindow.AssertGotoBoard("Test Board");

                    INavigationMenuWindow navMenuWindow = new NavigationMenuWindow(driver, _targetUri);

                    navMenuWindow.CreateMeeting();
                    navMenuWindow.AssertMeetingPopup();

                    navMenuWindow.FillAndConfirmMeeting();
                    navMenuWindow.AssertConfirmedMeeting();

                    _testOutputHelper.WriteLine($"[SUCCESS] {options.BrowserName} WebDriver successfully created a meeting.");

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
    }

    [Fact(Skip = "Not Implemented")]
    public async Task MeetingAgendaTest() {
        throw new NotImplementedException();
    }

    [Fact(Skip = "Not Implemented")]
    public async Task MeetingSummaryTest() {
        throw new NotImplementedException();
    }

    [Fact(Skip = "Not Implemented")]
    public async Task FileSearchTest() {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Projected to be a test of testing the scrive signature process feature.
    /// However there's a economic liability of executing this in a production-like environment.
    /// It's worth looking into SCRIVE's developer tools, to get a work-around.
    /// </summary>
    [Fact(Skip = "Economically non-viable")]
    public async Task SignatureProcessTest() {
        throw new NotImplementedException();
    }

    private static void ModifyDriverOptions(DriverOptions options)
    {
        var typeName = options.GetType().Name;
        if (typeName == nameof(ChromeOptions))
        {
            var chromeOptions = options as ChromeOptions;
            // Flags seems to work on all browsers https://github.com/GoogleChrome/chrome-launcher/blob/main/docs/chrome-flags-for-tools.md
            chromeOptions?.AddArguments(DEFAULT_WEBDRIVER_ARGUMENTS);
        }
        else if (typeName == nameof(FirefoxOptions))
        {
            var firefoxOptions = options as FirefoxOptions;
            firefoxOptions?.AddArguments(DEFAULT_WEBDRIVER_ARGUMENTS);
        }
        else if (typeName == nameof(EdgeOptions))
        {
            var edgeOptions = options as EdgeOptions;
            edgeOptions?.AddArguments(DEFAULT_WEBDRIVER_ARGUMENTS);
        }
    }
}