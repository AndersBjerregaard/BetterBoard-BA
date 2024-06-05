using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using WebDriverXUnit.Abstractions;
using WebDriverXUnit.Assertions;
using WebDriverXUnit.Assertions.Interfaces;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers.Interfaces;
using Xunit.Abstractions;

namespace WebDriverXUnit.WindowDrivers;

public class UnsignedDocumentsWindow(RemoteWebDriver driver, Uri baseUri, ITestOutputHelper testOutput) : IUnsignedDocumentsWindow
{
    private static readonly string URI_PATH = "#/unsigneddocuments";

    public void AssertNavigation()
    {
        driver.Url.Should().Be(baseUri.ToString() + URI_PATH);
    }

    public IUnsignedDocumentsAssertion AssertUnsignedDocuments(IWebElement table)
    {
        return new UnsginedDocumentsAssertion(table, testOutput);
    }

    public Result<IWebElement> GetUnsignedDocumentsTable()
    {
        var table = new IWebElementFinder(driver)
            .Find(By.TagName("table"));
        return table is not null ? Result<IWebElement>.Success(table) : Result<IWebElement>.Failure(new Exception("No table element found. It is possible that the current user has no unsigned documents."));
    }

    public void Navigate()
    {
        if (driver.Url == URI_PATH) {
            return;
        }
        driver.Navigate().GoToUrl(baseUri + URI_PATH);
    }
}