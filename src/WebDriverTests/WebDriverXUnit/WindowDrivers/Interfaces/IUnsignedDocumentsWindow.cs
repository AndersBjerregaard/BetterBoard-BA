using OpenQA.Selenium;
using WebDriverXUnit.Abstractions;
using WebDriverXUnit.Assertions.Interfaces;

namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface IUnsignedDocumentsWindow : IWindowDriver {
    IUnsignedDocumentsAssertion AssertUnsignedDocuments(IWebElement table);
    Result<IWebElement> GetUnsignedDocumentsTable();
}