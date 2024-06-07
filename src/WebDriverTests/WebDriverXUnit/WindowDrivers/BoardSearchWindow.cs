using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers.Interfaces;

namespace WebDriverXUnit.WindowDrivers;

public class BoardSearchWindow(RemoteWebDriver driver) : IBoardSearchWindow
{
    public void AssertSearch(string searchTerm)
    {
        var searchResultTable = new WebElementFinder(driver).Find(By.XPath("//table[@class='table table-striped table-emails table-hover']"));
        var resultBody = searchResultTable?.FindElement(By.TagName("tbody"));
        var result = resultBody?.FindElement(By.TagName("tr")); // Find first hit
        Assert.Contains(searchTerm.ToLower(), result?.GetDomProperty("outerText").ToLower());
    }
}