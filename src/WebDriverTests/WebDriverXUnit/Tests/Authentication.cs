using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using WebDriverXUnit.ClassData;
using WebDriverXUnit.Fixtures;
using WebDriverXUnit.Helpers;
using Xunit.Abstractions;

namespace WebDriverXUnit.Tests;

public class Authentication : IClassFixture<GridUri>
{
    private readonly GridUri _fixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public Authentication(GridUri fixture, ITestOutputHelper testOutputHelper)
    {
        _fixture = fixture;
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [ClassData(typeof(AuthenticationClassData))]
    public async Task Test1(DriverOptions driverOptions)
    {
        Task[] parallelTests = new Task[AvailableDriverOptions.GetAmount()];

        for (int i = 0; i < parallelTests.Length; i++)
        {
            Task task = Task.Run(async () => {
                RemoteWebDriver? driver = null;
                var uri = _fixture.WebDriverUri;
                try
                {
                    driver = new RemoteWebDriver(uri, driverOptions);

                    driver.Navigate().GoToUrl("https://dev.betterboard.dk/#/login");

                    Assert.Equal("BetterBoard - Board Management System", driver.Title);

                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                    wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
                    IWebElement? header = wait.Until(webDriver => {
                        var element = webDriver.FindElement(By.TagName("h4"));
                        return element.Displayed ? element : null;
                    });
                    Assert.NotNull(header);
                    Assert.Equal("Velkommen til BetterBoard", header.Text);

                    _testOutputHelper.WriteLine($"{driverOptions.BrowserName} WebDriver finished tests without errors");
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
}