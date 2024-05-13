using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using WebDriverXUnit.WindowDrivers.Interfaces;

namespace WebDriverXUnit.WindowDrivers;

public class BoardsWindow(RemoteWebDriver driver, Uri baseUri) : IBoardsWindow
{
    public void GoToBoard(string boardName)
    {
        var divs = driver.FindElements(By.ClassName("col-sm-6"));
        List<IWebElement> buttons = new List<IWebElement>();
        for (int i = 0; i < divs.Count; i++)
        {
            buttons.Add(divs[i].FindElement(By.ClassName("btn-transparent")));
        }
        Assert.True(buttons.Count == 1);
    }

    public void AssertGotoBoard(string boardName)
    {
        throw new NotImplementedException();
    }
}