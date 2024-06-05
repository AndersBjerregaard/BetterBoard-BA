using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using WebDriverXUnit.Abstractions;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers.Interfaces;
using Xunit.Abstractions;

namespace WebDriverXUnit.WindowDrivers;

public class BoardsAssertion(IWebElement board, ITestOutputHelper testOutput) : IBoardsAssertion
{
    public void HasUnreadDocuments()
    {
        Assert.Contains("Unread documents", board.GetDomProperty("innerText"));
        testOutput.WriteLine($"[Info] {nameof(this.HasUnreadDocuments)} executed...");
    }

    public void HasUnsignedDocuments()
    {
        var element = board.FindElement(By.XPath(".//span[@title='Documents awaiting your signature']"));
        Assert.NotNull(element);
        Assert.True(element.Displayed);
        testOutput.WriteLine($"[Info] {nameof(this.HasUnsignedDocuments)} executed...");
    }

    public void HasUpcomingMeeting()
    {
        var element = board.FindElement(By.XPath(".//i[@class='fa fa-calendar']"));
        Assert.NotNull(element);
        Assert.True(element.Displayed);
        testOutput.WriteLine($"[Info] {nameof(this.HasUpcomingMeeting)} executed...");
    }
}