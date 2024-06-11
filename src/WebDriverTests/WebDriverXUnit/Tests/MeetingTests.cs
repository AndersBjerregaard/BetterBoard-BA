using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using WebDriverXUnit.Domain;
using WebDriverXUnit.Extensions;
using WebDriverXUnit.Factories;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers;
using WebDriverXUnit.WindowDrivers.Interfaces;
using Xunit.Abstractions;

namespace WebDriverXUnit.Tests;

public class MeetingsTests(ITestOutputHelper testOutputHelper)
{

    private ITestOutputHelper _testOutputHelper = testOutputHelper;
    private readonly UserCredentials _testUserCredentials = TestVariablesFactory.GetUserCredentials();
    private readonly Uri _targetUri = TestVariablesFactory.GetSutUri();
    private readonly Uri _gridUri = TestVariablesFactory.GetSeleniumGridUri();
    private readonly string[] _defaultWebDriverArguments = TestVariablesFactory.DEFAULT_WEBDRIVER_ARGUMENTS;

    [Fact]
    public async Task MeetingCreationTest() {
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
                    loginWindow.FullLoginSession(_testUserCredentials);

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
            options.ApplyOptionsArguments(_defaultWebDriverArguments);
            Task task = Task.Run(() => {
                RemoteWebDriver? driver = null;
                try
                {
                    driver = new RemoteWebDriver(_gridUri, options);

                    ILoginWindow loginWindow = new LoginWindow(driver, _targetUri);
                    loginWindow.FullLoginSession(_testUserCredentials);

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
            options.ApplyOptionsArguments(_defaultWebDriverArguments);
            Task task = Task.Run(() => {
                RemoteWebDriver? driver = null;
                try
                {
                    driver = new RemoteWebDriver(_gridUri, options);

                    driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);

                    ILoginWindow loginWindow = new LoginWindow(driver, _targetUri);
                    loginWindow.FullLoginSession(_testUserCredentials);

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
                    driver?.Dump(options, _testOutputHelper);
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