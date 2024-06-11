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

public class FrontPageTests(ITestOutputHelper testOutputHelper)
{
    private ITestOutputHelper _testOutputHelper = testOutputHelper;
    private readonly UserCredentials _testUserCredentials = TestVariablesFactory.GetUserCredentials();
    private readonly Uri _targetUri = TestVariablesFactory.GetSutUri();
    private readonly Uri _gridUri = TestVariablesFactory.GetSeleniumGridUri();
    private readonly string[] _defaultWebDriverArguments = TestVariablesFactory.DEFAULT_WEBDRIVER_ARGUMENTS;

    [Fact]
    public async Task FrontPageTest() {
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
}