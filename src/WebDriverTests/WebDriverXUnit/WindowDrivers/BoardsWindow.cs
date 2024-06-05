using System.Collections.ObjectModel;
using System.Diagnostics;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using WebDriverXUnit.Abstractions;
using WebDriverXUnit.Assertions;
using WebDriverXUnit.Assertions.Interfaces;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers.Interfaces;
using Xunit.Abstractions;

namespace WebDriverXUnit.WindowDrivers;

public class BoardsWindow(RemoteWebDriver driver, Uri baseUri, ITestOutputHelper testOutput) : IBoardsWindow
{
    /// <summary>
    /// This method depends on the fact that the boards page renders a board & dataroom wrapped in a <div></div>.
    /// Thus the header and redirect button for each board & dataroom should have the same corresponding index, since they're children in the same div.
    /// </summary>
    public void GoToBoard(string boardName)
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
        testOutput.WriteLine($"[Info] {nameof(this.GoToBoard)} executed...");
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
        testOutput.WriteLine($"[Info] {nameof(this.Navigate)} executed...");
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
        testOutput.WriteLine($"[Info] {nameof(this.AssertNavigation)} executed...");
    }

    public IBoardsAssertion AssertBoard(IWebElement board)
    {
        return new BoardsAssertion(board, testOutput);
    }

    public Result<IWebElement> FindBoard(string boardName)
    {
        var boards = new IWebElementFinder(driver)
            .FindMultiple(By.XPath("//div[@class='widget-body clearfix']"));
        Assert.NotNull(boards);
        Assert.True(boards.Any());
        var board = boards.FirstOrDefault(x => x.GetDomProperty("innerText").Contains(boardName));
        testOutput.WriteLine($"[Info] {nameof(this.FindBoard)} executed...");
        return board is not null ? Result<IWebElement>.Success(board) : Result<IWebElement>.Failure(new Exception($"No board found with the name: {boardName}"));
    }

    public Result<IWebElement> FindBoard(string boardName, string companyName)
    {
        var boards = new IWebElementFinder(driver)
            .FindMultiple(By.XPath("//div[@class='widget-body clearfix']"));
        Assert.NotNull(boards);
        Assert.True(boards.Any());
        var board = boards.FirstOrDefault(x => x.GetDomProperty("innerText").Contains(boardName) && x.GetDomProperty("innerText").Contains(companyName));
        testOutput.WriteLine($"[Info] {nameof(this.FindBoard)} executed...");
        return board is not null ? Result<IWebElement>.Success(board) : Result<IWebElement>.Failure(new Exception($"No board found with the name: {boardName}. Under the company: {companyName}"));
    }

    public void SearchFor(string search)
    {
        var input = new IWebElementFinder(driver)
            .Find(By.XPath("//input[@class='filterInput']"));
        Assert.NotNull(input);
        var actions = new Actions(driver);
        actions.Click(input)
            .SendKeys(search)
            .Build()
            .Perform();
    }

    public void GoToUnsignedDocuments()
    {
        var hyperlink = new IWebElementFinder(driver)
            .Find(By.XPath("//a[@href='#/unsigneddocuments']"));
        Assert.NotNull(hyperlink);
        hyperlink.Click();
    }
}