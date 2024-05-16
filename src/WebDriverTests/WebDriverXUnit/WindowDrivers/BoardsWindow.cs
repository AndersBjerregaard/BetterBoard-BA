using System.Collections.ObjectModel;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using WebDriverXUnit.WindowDrivers.Interfaces;
using Xunit.Abstractions;

namespace WebDriverXUnit.WindowDrivers;

// Header: h3
// Redirect: btn-transparent
public class BoardsWindow(RemoteWebDriver driver, Uri baseUri) : IBoardsWindow
{
    public void GoToBoard(string boardName, ref ITestOutputHelper testOutput)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
        wait.IgnoreExceptionTypes([typeof(NoSuchElementException), typeof(StaleElementReferenceException)]);
        var btn = wait.Until(d => {
            var e = d.FindElement(By.TagName("button"));
            return e.Displayed ? e : null;
        });
        Assert.NotNull(btn);
        btn.Click();


        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
        wait.IgnoreExceptionTypes([typeof(NoSuchElementException), typeof(StaleElementReferenceException)]);
        var header = wait.Until(d => {
            var e = d.FindElement(By.TagName("h2"));
            return e.Displayed ? e : null;
        });
        Assert.NotNull(header);
        Assert.Contains("Godeftermiddag", header.Text);


        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
        wait.IgnoreExceptionTypes([typeof(NoSuchElementException), typeof(StaleElementReferenceException)]);
        var btns = wait.Until(d => {
            var e = d.FindElements(By.ClassName("btn-transparent"));
            return e.Any() ? e : null;
        });

        Assert.NotNull(btns);
        Assert.Equal(3, btns.Count);

        // var boardDivs = driver.FindElements(By.ClassName("col-sm-6"));
        // foreach (var boardDiv in boardDivs)
        // {
        //     var sections = boardDiv.FindElements(By.XPath("./child::*"));
        //     foreach (var section in sections)
        //     {
        //         var widgetBodies = boardDiv.FindElements(By.XPath("./child::*"));
        //         foreach (var widgetBody in widgetBodies)
        //         {
        //             var rows = boardDiv.FindElements(By.XPath("./child::*"));
        //             foreach (var row in rows)
        //             {
        //                 testOutput.WriteLine(row.ToString());
        //             }
        //         }
        //     }
        // }
    }

    public void AssertGotoBoard(string boardName)
    {
        throw new NotImplementedException();
    }

    public void Navigate()
    {
        driver.Navigate().GoToUrl(baseUri);
    }

    public void AssertNavigation()
    {
        throw new NotImplementedException();
    }
}