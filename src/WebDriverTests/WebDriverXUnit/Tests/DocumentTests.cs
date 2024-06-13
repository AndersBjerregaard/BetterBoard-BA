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

public class DocumentTests(ITestOutputHelper testOutputHelper)
{

    private ITestOutputHelper _testOutputHelper = testOutputHelper;
    private readonly UserCredentials _testUserCredentials = TestVariablesFactory.GetUserCredentials();
    private readonly Uri _targetUri = TestVariablesFactory.GetSutUri();
    private readonly Uri _gridUri = TestVariablesFactory.GetSeleniumGridUri();
    private readonly string _shortId = TestVariablesFactory.GetShortId();
    private readonly string[] _defaultWebDriverArguments = TestVariablesFactory.DEFAULT_WEBDRIVER_ARGUMENTS;

    [Fact]
    public async Task FileSearchTest() {
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

                    var result = boardsWindow.FindBoard($"Board {_shortId}", $"Company {_shortId}");
                    Assert.True(result.IsSuccess);
                    var board = result.GetValueOrThrow();
                    boardsWindow.GoToBoard(board);
                    boardsWindow.AssertGotoBoard($"Board {_shortId}", $"Company {_shortId}");

                    INavBarWindow navbarWindow = new NavBarWindow(driver);
                    navbarWindow.DocumentSearch($"Doc {_shortId}");
                    
                    IBoardSearchWindow boardSearch = new BoardSearchWindow(driver);
                    boardSearch.AssertSearch($"Doc {_shortId}");
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

                    var result = boardsWindow.FindBoard($"Board {_shortId}", $"Company {_shortId}");
                    Assert.True(result.IsSuccess);
                    var board = result.GetValueOrThrow();
                    boardsWindow.GoToBoard(board);
                    boardsWindow.AssertGotoBoard($"Board {_shortId}", $"Company {_shortId}");

                    INavigationMenuWindow navMenu = new NavigationMenuWindow(driver);
                    navMenu.OpenCompanyDocuments(options.BrowserName.AsSpan());

                    ICompanyDocsWindow companyDocs = new CompanyDocsWindow(driver);
                    companyDocs.AssertPage();
                    companyDocs.OpenFolder($"Folder {_shortId}");
                    companyDocs.OpenSignProcessModal(documentName: $"Signdoc {_shortId}");

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

}