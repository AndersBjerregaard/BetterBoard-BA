using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using WebDriverXUnit.Domain;
using WebDriverXUnit.Fixtures;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers;
using WebDriverXUnit.WindowDrivers.Interfaces;
using Xunit.Abstractions;

namespace WebDriverXUnit.Tests;

public class LoginTests : IClassFixture<TestVariables>
{
    private readonly TestVariables _fixture;
    private ITestOutputHelper _testOutputHelper;
    private readonly UserCredentials _testUserCredentials;
    private readonly Uri _targetUri;
    private readonly Uri _gridUri;
    private static readonly string[] DEFAULT_WEBDRIVER_ARGUMENTS = ["--no-sandbox", "--disable-dev-shm-usage", "--incognito", "--window-size=1920,1080"];

    public LoginTests(TestVariables fixture, ITestOutputHelper testOutputHelper)
    {
        _fixture = fixture;
        _testOutputHelper = testOutputHelper;

        _testUserCredentials = _fixture.TestUuid ?? throw new NullReferenceException();

        _targetUri = _fixture.TargetUri ?? throw new NullReferenceException();

        _gridUri = _fixture.WebDriverUri ?? throw new NullReferenceException();
    }

    [Fact(Skip = "Obselete")]
    public void FirefoxSmokeTest()
    {
        var options = new FirefoxOptions();
        ApplyOptionArguments(options);

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
        DriverOptions[] driverOptions = AvailableDriverOptions.Get();
        Task[] parallelTests = new Task[driverOptions.Length];
        bool failed = false;

        for (int i = 0; i < driverOptions.Length; i++)
        {
            DriverOptions options = driverOptions[i];
            ApplyOptionArguments(options);
            Task task = Task.Run(() =>
            {
                RemoteWebDriver? driver = null;
                try
                {
                    driver = new RemoteWebDriver(_gridUri, options);

                    Login(driver);
                    
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

    [Fact]
    public async Task FrontPageTest() {
        DriverOptions[] driverOptions = AvailableDriverOptions.Get();
        Task[] parallelTests = new Task[driverOptions.Length];
        bool failed = false;

        for (int i = 0; i < driverOptions.Length; i++)
        {
            DriverOptions options = driverOptions[i];
            ApplyOptionArguments(options);
            Task task = Task.Run(() =>
            {
                RemoteWebDriver? driver = null;
                try
                {
                    driver = new RemoteWebDriver(_gridUri, options);

                    Login(driver);

                    IBoardsWindow boardsWindow = new BoardsWindow(driver, _targetUri, _testOutputHelper);

                    boardsWindow.Navigate();
                    boardsWindow.AssertNavigation();

                    // Boards
                    // Unread & Unsigned Documents
                    var result = boardsWindow.FindBoard("Bestyrelsen", "Anders Test ApS");
                    Assert.True(result.IsSuccess);
                    var board = result.GetValueOrThrow();
                    boardsWindow.AssertBoard(board)
                        .IsABoard()
                        .HasUnreadDocuments()
                        .HasUnsignedDocuments();
                    
                    // Upcoming Meeting
                    result = boardsWindow.FindBoard("Bestyrelsen", "BetterBoard ApS");
                    Assert.True(result.IsSuccess);
                    board = result.GetValueOrThrow();
                    boardsWindow.AssertBoard(board)
                        .IsABoard()
                        .HasUpcomingMeeting();

                    // Dataroom
                    // Unread & Unsigned Documents
                    result = boardsWindow.FindBoard("Kun Datarum", "Anders Datarum");
                    Assert.True(result.IsSuccess);
                    board = result.GetValueOrThrow();
                    boardsWindow.AssertBoard(board)
                        .IsADataroom()
                        .HasUnreadDocuments()
                        .HasUnsignedDocuments();

                    // Search
                    boardsWindow.SearchFor("Anders Datarum");
                    result = boardsWindow.FindBoard("Kun Datarum", "Anders Datarum");
                    Assert.True(result.IsSuccess);
                    result = boardsWindow.FindBoard("Bestyrelsen", "Anders Test ApS"); // Exists
                    Assert.True(result.IsFailure); // Assert search hides other boards

                    // Unsigned Documents
                    boardsWindow.GoToUnsignedDocuments();

                    IUnsignedDocumentsWindow docsWindow = new UnsignedDocumentsWindow(driver, _targetUri);

                    docsWindow.AssertNavigation();

                    result = docsWindow.GetUnsignedDocumentsTable();
                    Assert.True(result.IsSuccess);
                    docsWindow.AssertUnsignedDocuments(result.GetValueOrThrow())
                        .HasDocumentFromOrigin("Kun Datarum", "Anders Test ApS");
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

    [Fact]
    public async Task MeetingCreationTest() {
        DriverOptions[] driverOptions = AvailableDriverOptions.Get();
        Task[] parallelTests = new Task[driverOptions.Length];
        bool failed = false;

        for (int i = 0; i < driverOptions.Length; i++)
        {
            DriverOptions options = driverOptions[i];
            ApplyOptionArguments(options);
            Task task = Task.Run(() =>
            {
                RemoteWebDriver? driver = null;
                try
                {
                    driver = new RemoteWebDriver(_gridUri, options);

                    Login(driver);

                    IBoardsWindow boardsWindow = new BoardsWindow(driver, _targetUri, _testOutputHelper);

                    boardsWindow.Navigate();
                    boardsWindow.AssertNavigation();

                    boardsWindow.GoToBoard("Test Board");
                    boardsWindow.AssertGotoBoard("Test Board");

                    INavigationMenuWindow navMenuWindow = new NavigationMenuWindow(driver);

                    navMenuWindow.CreateMeeting(ref _testOutputHelper, options.BrowserName.AsSpan());
                    navMenuWindow.AssertMeetingPopup(ref _testOutputHelper, options.BrowserName.AsSpan());

                    ICreateMeetingWindow meetingWindow = new CreateMeetingWindow(driver);

                    var result = meetingWindow.FillAndConfirmMeeting(ref _testOutputHelper, options.BrowserName.AsSpan());
                    Assert.True(result.IsSuccess);
                    meetingWindow.AssertMeetingConfirmed(result.GetValueOrThrow(), ref _testOutputHelper, options.BrowserName.AsSpan());

                    _testOutputHelper.WriteLine($"[SUCCESS] {options.BrowserName} WebDriver successfully created meeting {result.GetValueOrThrow()}.");

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

    [Fact]
    public async Task MeetingAgendaTest() {
        DriverOptions[] driverOptions = AvailableDriverOptions.Get();
        Task[] parallelTests = new Task[driverOptions.Length];
        bool failed = false;
        for (int i = 0; i < driverOptions.Length; i++)
        {
            DriverOptions options = driverOptions[i];
            ApplyOptionArguments(options);
            Task task = Task.Run(() => {
                RemoteWebDriver? driver = null;
                try
                {
                    driver = new RemoteWebDriver(_gridUri, options);

                    Login(driver);

                    IBoardsWindow boardsWindow = new BoardsWindow(driver, _targetUri, _testOutputHelper);

                    boardsWindow.Navigate();
                    boardsWindow.AssertNavigation();

                    var result = boardsWindow.FindBoard("Bestyrelsen", "Anders Test ApS");
                    Assert.True(result.IsSuccess);
                    var board = result.GetValueOrThrow();
                    boardsWindow.AssertBoard(board)
                        .IsABoard()
                        .HasUpcomingMeeting();
                    boardsWindow.GoToBoard(board);
                    boardsWindow.AssertGotoBoard("Bestyrelsen", "Anders Test ApS");

                    IMeetingWindow meetingWindow = new MeetingWIndow(driver, _testOutputHelper);

                    meetingWindow.AssertCurrentViewedMeeting("General boardmeeting");

                    var section = meetingWindow.GetMeetingAgendaSection();
                    meetingWindow.UploadDocumentToFirstAgendaItem(section);

                    _testOutputHelper.WriteLine($"[SUCCESS] {options.BrowserName} successfully executed {nameof(MeetingAgendaTest)}");
                }
                catch (Exception e) {
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

    [Fact]
    public async Task MeetingSummaryTest() {
        DriverOptions[] driverOptions = AvailableDriverOptions.FIREFOX_OPTIONS;
        Task[] parallelTests = new Task[driverOptions.Length];
        bool failed = false;
        for (int i = 0; i < driverOptions.Length; i++)
        {
            DriverOptions options = driverOptions[i];
            ApplyOptionArguments(options);
            Task task = Task.Run(() => {
                RemoteWebDriver? driver = null;
                try
                {
                    driver = new RemoteWebDriver(_gridUri, options);

                    driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);

                    Login(driver);

                    IBoardsWindow boardsWindow = new BoardsWindow(driver, _targetUri, _testOutputHelper);

                    boardsWindow.Navigate();
                    boardsWindow.AssertNavigation();

                    var result = boardsWindow.FindBoard("Bestyrelsen", "Anders Test ApS");
                    Assert.True(result.IsSuccess);
                    var board = result.GetValueOrThrow();
                    boardsWindow.AssertBoard(board)
                        .IsABoard()
                        .HasUpcomingMeeting();
                    boardsWindow.GoToBoard(board);
                    boardsWindow.AssertGotoBoard("Bestyrelsen", "Anders Test ApS");

                    IMeetingWindow meetingWindow = new MeetingWIndow(driver, _testOutputHelper);

                    meetingWindow.AssertCurrentViewedMeeting("General boardmeeting");

                    var agendaSection = meetingWindow.GetMeetingAgendaSection();
                    var tinymceFrame = meetingWindow.GetSummaryOfFirstAgendaItem(agendaSection);

                    driver.SwitchTo().Frame(tinymceFrame);
                    
                    ITinyMceWindow tinymceWindow = new TinyMceWindow(driver, _testOutputHelper);
                    tinymceWindow.WriteParagraph("lorem ipsum");

                    driver.SwitchTo().ParentFrame();
                    
                    meetingWindow.SaveSummary(agendaSection);
                }
                catch (Exception e) {
                    ExceptionLogger.LogException(e, ref _testOutputHelper, options.BrowserName);
                    failed = true;
                    Dump(driver, options.BrowserName);
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

    [Fact]
    public async Task FileSearchTest() {
        DriverOptions[] driverOptions = AvailableDriverOptions.Get();
        Task[] parallelTests = new Task[driverOptions.Length];
        bool failed = false;
        for (int i = 0; i < driverOptions.Length; i++)
        {
            DriverOptions options = driverOptions[i];
            ApplyOptionArguments(options);
            Task task = Task.Run(() => {
                RemoteWebDriver? driver = null;
                try
                {
                    driver = new RemoteWebDriver(_gridUri, options);

                    Login(driver);

                    IBoardsWindow boardsWindow = new BoardsWindow(driver, _targetUri, _testOutputHelper);

                    boardsWindow.Navigate();
                    boardsWindow.AssertNavigation();

                    var result = boardsWindow.FindBoard("Bestyrelsen", "BetterBoard ApS");
                    Assert.True(result.IsSuccess);
                    var board = result.GetValueOrThrow();
                    boardsWindow.GoToBoard(board);
                    boardsWindow.AssertGotoBoard("Bestyrelsen", "BetterBoard ApS");

                    INavBarWindow navbarWindow = new NavBarWindow(driver);
                    navbarWindow.DocumentSearch("referat");
                    
                    IBoardSearchWindow boardSearch = new BoardSearchWindow(driver);
                    boardSearch.AssertSearch("referat");
                }
                catch (Exception e) {
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

    [Fact]
    public async Task SignatureProcessTest() {
        
        DriverOptions[] driverOptions = AvailableDriverOptions.Get();
        Task[] parallelTests = new Task[driverOptions.Length];
        bool failed = false;
        for (int i = 0; i < driverOptions.Length; i++)
        {
            DriverOptions options = driverOptions[i];
            ApplyOptionArguments(options);
            Task task = Task.Run(() => {
                RemoteWebDriver? driver = null;
                try
                {
                    driver = new RemoteWebDriver(_gridUri, options);

                    Login(driver);

                    IBoardsWindow boardsWindow = new BoardsWindow(driver, _targetUri, _testOutputHelper);

                    boardsWindow.Navigate();
                    boardsWindow.AssertNavigation();

                    var result = boardsWindow.FindBoard("Bestyrelsen", "Anders Test ApS");
                    Assert.True(result.IsSuccess);
                    var board = result.GetValueOrThrow();
                    boardsWindow.GoToBoard(board);
                    boardsWindow.AssertGotoBoard("Bestyrelsen", "Anders Test ApS");

                    INavigationMenuWindow navMenu = new NavigationMenuWindow(driver);
                    navMenu.OpenCompanyDocuments(options.BrowserName.AsSpan());

                    ICompanyDocsWindow companyDocs = new CompanyDocsWindow(driver);
                    companyDocs.AssertPage();
                    companyDocs.OpenFolder("dokumenter");
                    companyDocs.OpenSignProcessModal(documentName: "logo");

                    ISignatureProcessModalWindow modal = new SignatureProcessModalWindow(driver);
                    modal.AssertModal();
                }
                catch (Exception e) {
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

    private static void ApplyOptionArguments(DriverOptions options)
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
            edgeOptions?.AddArguments("--no-sandbox", "--disable-dev-shm-usage", "--guest", "--headless", "--window-size=1920,1080"); // Guest argument to disable personalized edge pop-ups
        }
    }

    private void Login(RemoteWebDriver driver) {
        ILoginWindow loginWindow = new LoginWindow(driver, _targetUri);
        loginWindow.Navigate();
        loginWindow.AssertNavigation();
        loginWindow.Login(_testUserCredentials);
        loginWindow.AssertLogin(_testUserCredentials);
        driver.ExecuteScript("localStorage.setItem(arguments[0],arguments[1])", "purechat_expanded", "false"); // Disable help pop-up
    }

    private void Dump(RemoteWebDriver? driver, string browserName) {
        var scrShot = driver?.GetScreenshot();
        var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string path = $"./dump/";
        scrShot?.SaveAsFile(path + $"{browserName}-{time}.png");
        _testOutputHelper.WriteLine("[LOG] Saved screenshot");
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + path + $"{browserName}-{time}.html"))) {
            var html = driver?.PageSource;
            outputFile.WriteLine(html);
        }
        _testOutputHelper.WriteLine("[LOG] Dumped HTML");
    }
}