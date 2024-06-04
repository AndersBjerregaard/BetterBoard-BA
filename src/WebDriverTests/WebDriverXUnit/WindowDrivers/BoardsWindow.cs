using System.Collections.ObjectModel;
using System.Diagnostics;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers.Interfaces;
using Xunit.Abstractions;

namespace WebDriverXUnit.WindowDrivers;

public class BoardsWindow(RemoteWebDriver driver, Uri baseUri) : IBoardsWindow
{
    /// <summary>
    /// This method depends on the fact that the boards page renders a board & dataroom wrapped in a <div></div>.
    /// Thus the header and redirect button for each board & dataroom should have the same corresponding index, since they're children in the same div.
    /// </summary>
    public void GoToBoard(string boardName, ref ITestOutputHelper testOutput)
    {
        // Find board name's index
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        wait.IgnoreExceptionTypes([typeof(NoSuchElementException), typeof(StaleElementReferenceException)]);
        var headers = wait.Until(d =>
        {
            var e = d.FindElements(By.TagName("h4"));
            return e.Any() ? e : null;
        });
        Assert.NotNull(headers);
        int? headerIndex = null;
        for (var i = 0; i < headers.Count; i++)
        {
            var e = headers[i];
            if (e.Text.Contains(boardName))
            {
                headerIndex = i;
            }
        }
        Assert.NotNull(headerIndex);

        // Find board redirect by index
        var btns = driver.FindElements(By.ClassName("btn-transparent"));
        Assert.NotNull(btns);
        var btn = btns[headerIndex.Value];
        Assert.NotNull(btn);
        btn.Click();
    }

    public void AssertGotoBoard(string boardName)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        var small = wait.Until(driver => {
            var element = driver.FindElement(By.TagName("small"));
            return element.Displayed ? element : null;
        });
        Assert.NotNull(small);
        Assert.Contains(boardName, small.Text);
    }

    public void Navigate()
    {
        if (driver.Url == baseUri + "#/boards") {
            return;
        }
        driver.Navigate().GoToUrl(baseUri + "#/boards");
    }

    public void AssertNavigation()
    {
        driver.Url.Should().Be(baseUri.ToString() + "#/boards");
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        var paragraph = wait.Until(driver => {
            var element = driver.FindElement(By.TagName("p"));
            return element.Displayed ? element : null;
        });
        Assert.NotNull(paragraph);
        var text = paragraph.Text;
        text.Should().BeOneOf(
            "Welcome to your BetterBoard home screen",
            "Velkommen til BetterBoard"
        );
    }

    public IBoardsAssertion AssertBoardHas(string boardName, ITestOutputHelper _testOutputHelper)
    {
        var boards = new IWebElementFinder(driver)
            .FindMultiple(By.XPath("//div[@class='widget-body clearfix']"));
        Assert.NotNull(boards);
        Assert.True(boards.Any());
        var board = boards.First(x => x.GetDomProperty("innerText").Contains(boardName));
        Assert.NotNull(board);
        Assert.Contains("Unread documents", board.GetDomProperty("innerText"));
        return new BoardsAssertion();
    }
}