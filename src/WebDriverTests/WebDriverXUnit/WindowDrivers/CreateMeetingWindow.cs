using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using WebDriverXUnit.Abstractions;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers.Interfaces;

namespace WebDriverXUnit.WindowDrivers;

public class CreateMeetingWindow(RemoteWebDriver driver, Uri baseUri) : ICreateMeetingWindow
{
    public void AssertMeetingConfirmed(string meetingTitle, ref Xunit.Abstractions.ITestOutputHelper _testOutputHelper, ReadOnlySpan<char> browserName)
    {
        var element = new IWebElementFinder(driver)
            .Find(By.XPath("//span[@id='meetingTitle']"));
        Assert.NotNull(element);
        element.Text.Should().Be(meetingTitle);
        _testOutputHelper.WriteLine($"[INFO] {browserName} Assert Meeting Confirmed");
    }

    public Result<string> FillAndConfirmMeeting(ref Xunit.Abstractions.ITestOutputHelper _testOutputHelper, ReadOnlySpan<char> browserName)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        var inputFields = wait.Until(d => {
            var e = driver.FindElements(By.XPath("//input[@class='form-control']"));
            return e.Any() ? e : null;
        });

        string meetingTitle = "General boardmeeting " + Guid.NewGuid().ToString()[^8..];

        inputFields[0].SendKeys(meetingTitle); // Title
        inputFields[1].SendKeys("Company HQ"); // Location

        var datePicker = driver.FindElement(By.XPath("//input[@id='dateStart']"));
        datePicker.Click();
        var today = driver.FindElement(By.XPath("//span[@class='cell day today']"));
        today.Click();

        var timePickers = driver.FindElements(By.XPath("//input[@type='time']"));
        var actions = new Actions(driver);
        actions.Click(timePickers[0]) // Start time
            .SendKeys(Keys.ArrowLeft)
            .SendKeys(Keys.ArrowLeft)
            .SendKeys("1159PM")
            .Build()
            .Perform();
        actions.Click(timePickers[1]) // End time
            .SendKeys(Keys.ArrowLeft)
            .SendKeys(Keys.ArrowLeft)
            .SendKeys("1159PM")
            .Build()
            .Perform();

        var submit = driver.FindElement(By.ClassName("btn-primary"));
        Assert.Equal("Create", submit.Text);
        submit.Click();

        _testOutputHelper.WriteLine($"[INFO] {browserName} Fill and Confirm Meeting.");

        return Result<string>.Success(meetingTitle);
    }
}