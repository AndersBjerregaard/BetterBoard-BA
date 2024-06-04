using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using WebDriverXUnit.Abstractions;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers.Interfaces;

namespace WebDriverXUnit.WindowDrivers;

public class BoardsAssertion(IWebElement board, RemoteWebDriver driver) : IBoardsAssertion
{
    public void HasUnreadDocuments()
    {
        Assert.Contains("Unread documents", board.GetDomProperty("innerText"));
    }

    public void HasUnsignedDocuments()
    {
        var element = new IWebElementFinder(driver)
            .Find(By.XPath(".//span[@title='Documents awaiting your signature']"));
        Assert.NotNull(element);
    }

    public void HasUpcomingMeeting()
    {
        var element = new IWebElementFinder(driver)
            .Find(By.XPath(".//i[@class='fa fa-calendar']"));
        Assert.NotNull(element);
    }
}