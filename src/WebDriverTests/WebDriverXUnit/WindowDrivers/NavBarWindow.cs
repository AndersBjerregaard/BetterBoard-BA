using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers.Interfaces;

namespace WebDriverXUnit.WindowDrivers;

public class NavBarWindow(RemoteWebDriver driver) : INavBarWindow
{
    public void DocumentSearch(string searchTerm)
    {
        var navbar = new WebElementFinder(driver).Find(By.XPath("//ul[@class='nav navbar-nav navbar-right']"));
        var searchInput = navbar?.FindElement(By.XPath(".//input[@class='form-control' and @type='text']"));
        new Actions(driver)
            .Click(searchInput)
            .SendKeys(searchTerm)
            .SendKeys(Keys.Enter)
            .Build()
            .Perform();
    }
}