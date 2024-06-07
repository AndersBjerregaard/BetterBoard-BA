using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers.Interfaces;

namespace WebDriverXUnit.WindowDrivers;

public class BoardSearchWindow(RemoteWebDriver driver) : IBoardSearchWindow
{
    public void AssertSearch(string searchTerm)
    {
        var searchResultTable = new WebElementFinder(driver).Find(By.XPath("//table[@class='table table-striped table-emails table-hover']"));
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
        var resultBody = wait.Until(d => {
            var e = searchResultTable?.FindElement(By.TagName("tbody"));
            return e is not null ? e : null;
        });
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
        var result = wait.Until(d => {
            var e = resultBody?.FindElement(By.TagName("tr")); // Find first hit
            return e is not null ? e : null;
        });
        Assert.Contains(searchTerm.ToLower(), result?.GetDomProperty("outerText").ToLower());
    }
}