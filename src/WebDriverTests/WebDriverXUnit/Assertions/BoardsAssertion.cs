using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using WebDriverXUnit.Abstractions;
using WebDriverXUnit.Assertions.Interfaces;
using WebDriverXUnit.Helpers;
using Xunit.Abstractions;

namespace WebDriverXUnit.Assertions;

public class BoardsAssertion(IWebElement board, ITestOutputHelper testOutput) : IBoardsAssertion
{
    public IBoardsAssertion HasUnreadDocuments()
    {
        Assert.Contains("Unread documents", board.GetDomProperty("innerText"));
        testOutput.WriteLine($"[Info] {nameof(this.HasUnreadDocuments)} executed...");
        return this;
    }

    public IBoardsAssertion HasNoUnreadDocuments() {
        string innerText = board.GetDomProperty("innerText");
        Assert.Contains("OK", innerText);
        Assert.DoesNotContain("Unread documents", innerText);
        return this;
    }

    public IBoardsAssertion HasUnsignedDocuments()
    {
        var element = board.FindElement(By.XPath(".//span[@title='Documents awaiting your signature']"));
        Assert.NotNull(element);
        Assert.True(element.Displayed);
        testOutput.WriteLine($"[Info] {nameof(this.HasUnsignedDocuments)} executed...");
        return this;
    }

    public IBoardsAssertion HasNoUnsignedDocuments() {

        Assert.Throws<NoSuchElementException>( () => board.FindElement(By.XPath(".//span[@title='Documents awaiting your signature']")));
        return this;
    }

    public IBoardsAssertion HasUpcomingMeeting()
    {
        var element = board.FindElement(By.XPath(".//i[@class='fa fa-calendar']"));
        Assert.NotNull(element);
        Assert.True(element.Displayed);
        testOutput.WriteLine($"[Info] {nameof(this.HasUpcomingMeeting)} executed...");
        return this;
    }

    public IBoardsAssertion HasNoUpcomingMeeting() {

        var element = board.FindElement(By.XPath(".//i[@class='fa fa-calendar']"));
        Assert.False(element.Displayed);
        return this;
    }

    public IBoardsAssertion IsABoard()
    {
        var icon = board.FindElement(By.XPath(".//span[@class='widget-icon']"));
        Assert.Contains("fas fa-building", icon.GetDomProperty("innerHTML"));
        testOutput.WriteLine($"[Info] {nameof(this.IsABoard)} executed...");
        return this;
    }

    public IBoardsAssertion IsADataroom()
    {
        var icon = board.FindElement(By.XPath(".//span[@class='widget-icon']"));
        Assert.Contains("fas fa-database", icon.GetDomProperty("innerHTML"));
        testOutput.WriteLine($"[Info] {nameof(this.IsADataroom)} executed...");
        return this;
    }
}