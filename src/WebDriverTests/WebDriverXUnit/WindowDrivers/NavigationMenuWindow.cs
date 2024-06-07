using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using WebDriverXUnit.Abstractions;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers.Interfaces;

namespace WebDriverXUnit.WindowDrivers;

public class NavigationMenuWindow(RemoteWebDriver driver) : INavigationMenuWindow
{
    public void AssertMeetingPopup(ref Xunit.Abstractions.ITestOutputHelper _testOutputHelper, ReadOnlySpan<char> browserName)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        var header = wait.Until(d => {
            var e = driver.FindElement(By.XPath("//h5[text()='Add new meeting']"));
            return e.Displayed ? e : null;
        });
        Assert.NotNull(header);
        _testOutputHelper.WriteLine($"[INFO] {browserName} Assert Meeting Popup.");
    }

    public void CreateMeeting(ref Xunit.Abstractions.ITestOutputHelper _testOutputHelper, ReadOnlySpan<char> browserName)
    {
        ExpandSideBar(browserName);
        // Open 'create meeting' modal
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        var hyperlink = wait.Until(d => {
            var e = driver.FindElement(By.XPath("//a[@title='Create new meeting']"));
            return e.Displayed ? e : null;
        });
        Assert.NotNull(hyperlink);
        hyperlink.Click();
        _testOutputHelper.WriteLine($"[INFO] {browserName} Create Meeting Popup.");
    }

    public void OpenCompanyDocuments(ReadOnlySpan<char> browserName)
    {
        ExpandSideBar(browserName);
        var sidebar = new WebElementFinder(driver).Find(By.XPath("//ul[@class='sidebar-nav']"));
        var folders = sidebar?.FindElement(By.XPath(".//a[@href='#sidebar-betterboardmenu-categories']"));
        folders?.Click();
        var categories = sidebar?.FindElements(By.XPath(".//ul[@id='sidebar-betterboardmenu-categories']/li"));
        Assert.NotNull(categories);
        IWebElement? companyDocs = null;
        foreach (var e in categories)
        {
            if (e.Text.ToLower().Contains("company documents")) {
                companyDocs = e;
                break;
            }
        }
        companyDocs?.Click();
    }

    private void ExpandSideBar(ReadOnlySpan<char> browserName) {
        // Expand sidebar
        if (browserName == "firefox")
        {
            var driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driverWait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException), typeof(ElementNotInteractableException));
            driverWait.Until(d =>
            {
                var e = driver.FindElement(By.XPath("//a[@id='nav-state-toggle']"));
                e.Click();
                return true;
            });
        }
        else // Chromium browsers
        {
            var driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driverWait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException), typeof(ElementNotInteractableException));
            driverWait.Until(d =>
            {
                var e = driver.FindElement(By.XPath("//a[@id='nav-collapse-toggle']"));
                e.Click();
                return true;
            });
        }
    }
}