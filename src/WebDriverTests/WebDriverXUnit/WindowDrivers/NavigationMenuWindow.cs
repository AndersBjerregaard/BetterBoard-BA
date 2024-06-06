using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using WebDriverXUnit.Abstractions;
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
        // Expand sidebar
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        var burgermenu = wait.Until(d => {
            var e = driver.FindElement(By.XPath("//a[@id='nav-state-toggle']"));
            return e.Enabled ? e : null;
        });
        Assert.NotNull(burgermenu);
        burgermenu.Click();

        // Open 'create meeting' modal
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        var hyperlink = wait.Until(d => {
            var e = driver.FindElement(By.XPath("//a[@title='Create new meeting']"));
            return e.Displayed ? e : null;
        });
        Assert.NotNull(hyperlink);
        hyperlink.Click();
        _testOutputHelper.WriteLine($"[INFO] {browserName} Create Meeting Popup.");
    }
}