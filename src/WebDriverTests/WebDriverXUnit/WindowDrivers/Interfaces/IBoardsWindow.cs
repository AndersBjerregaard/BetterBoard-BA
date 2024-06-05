using OpenQA.Selenium;
using WebDriverXUnit.Abstractions;
using WebDriverXUnit.Assertions.Interfaces;
using Xunit.Abstractions;

namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface IBoardsWindow : IWindowDriver {
    void GoToBoard(string boardName);
    void GoToBoard(IWebElement board);
    void AssertGotoBoard(string boardName);
    void AssertGotoBoard(string boardName, string companyName);
    Result<IWebElement> FindBoard(string boardName);
    Result<IWebElement> FindBoard(string boardName, string companyName);
    IBoardsAssertion AssertBoard(IWebElement board);
    void SearchFor(string search);
    void GoToUnsignedDocuments();
}