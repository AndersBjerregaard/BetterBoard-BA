using System.Collections.ObjectModel;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using WebDriverXUnit.WindowDrivers.Interfaces;
using Xunit.Abstractions;

namespace WebDriverXUnit.WindowDrivers;

// Header: h3
// Redirect: btn-transparent
public class BoardsWindow(RemoteWebDriver driver, Uri baseUri) : IBoardsWindow
{
    public void GoToBoard(string boardName, ref ITestOutputHelper testOutput)
    {
        var boardDivs = driver.FindElements(By.ClassName("col-sm-6"));
        foreach (var boardDiv in boardDivs)
        {
            var sections = boardDiv.FindElements(By.XPath("./child::*"));
            foreach (var section in sections)
            {
                var widgetBodies = boardDiv.FindElements(By.XPath("./child::*"));
                foreach (var widgetBody in widgetBodies)
                {
                    var rows = boardDiv.FindElements(By.XPath("./child::*"));
                    foreach (var row in rows)
                    {
                        testOutput.WriteLine(row.ToString());
                    }
                }
            }
        }
    }

    public void AssertGotoBoard(string boardName)
    {
        throw new NotImplementedException();
    }
}