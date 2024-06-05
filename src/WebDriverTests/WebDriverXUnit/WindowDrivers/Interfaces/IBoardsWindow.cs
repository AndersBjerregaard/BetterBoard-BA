using OpenQA.Selenium;
using WebDriverXUnit.Abstractions;
using WebDriverXUnit.Assertions.Interfaces;
using Xunit.Abstractions;

namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface IBoardsWindow : IWindowDriver {
    void GoToBoard(string boardName);
    void AssertGotoBoard(string boardName);
    Result<IWebElement> FindBoard(string boardName);
    Result<IWebElement> FindBoard(string boardName, string companyName);
    IBoardsAssertion AssertBoard(IWebElement board);
    void SearchFor(string search);
    void GoToUnsignedDocuments();
}