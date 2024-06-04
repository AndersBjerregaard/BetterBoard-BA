using OpenQA.Selenium;
using WebDriverXUnit.Abstractions;
using Xunit.Abstractions;

namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface IBoardsWindow : IWindowDriver {
    void GoToBoard(string boardName);
    void AssertGotoBoard(string boardName);
    Result<IWebElement> FindBoard(string boardName);
    Result<IWebElement> FindBoard(string boardName, string companyName);
    IBoardsAssertion AssertBoard(IWebElement board);
}