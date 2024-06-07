using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers.Interfaces;

namespace WebDriverXUnit.WindowDrivers;

public class CompanyDocsWindow(RemoteWebDriver driver) : ICompanyDocsWindow
{
    public void AssertPage()
    {
        var header = new WebElementFinder(driver).Find(By.TagName("h1"));
        Assert.Equal("company documents", header?.Text.Trim().ToLower());
    }

    public void OpenFolder(string folderName)
    {
        var folders = new WebElementFinder(driver).FindMultiple(By.XPath("//section[@class='widget bg-secondary text-white']"));
        Assert.NotNull(folders);
        IWebElement? folder = null;
        foreach (var f in folders)
        {
            if (f.GetDomProperty("outerText").ToLower().Contains(folderName)) {
                folder = f;
                break;
            }
        }
        Assert.NotNull(folder);
        var openBtn = folder.FindElement(By.XPath(".//button[@class='btn btn-secondary']"));
        openBtn?.Click();
    }

    public void OpenSignProcessModal(string documentName)
    {
        var documentsTable = new WebElementFinder(driver).Find(By.XPath("//table[@id='folder-view']"));
        var documents = documentsTable?.FindElements(By.TagName("tr"));
        var hit = documents?.FirstOrDefault(d => d.GetDomProperty("innerText").ToLower().Contains(documentName));
        Assert.NotNull(hit);
        var sigBtn = hit.FindElement(By.XPath(".//a[@title='Start signature process']"));
        sigBtn?.Click();
    }
}