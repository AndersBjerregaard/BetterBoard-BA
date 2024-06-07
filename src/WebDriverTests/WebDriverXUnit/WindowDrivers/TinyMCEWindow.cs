using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers.Interfaces;
using Xunit.Abstractions;

namespace WebDriverXUnit.WindowDrivers;

public class TinyMceWindow(RemoteWebDriver driver, ITestOutputHelper testOutput) : ITinyMceWindow
{
    public void WriteParagraph(string p)
    {
        var paragraph = new WebElementFinder(driver).Find(By.TagName("p"));
        Assert.NotNull(paragraph);
        Assert.Equal("True", paragraph.GetDomProperty("isContentEditable"));

        testOutput.WriteLine("[LOG] Found editable tinymce p element");

        paragraph.Clear();

        new Actions(driver)
            .Click(paragraph)
            .SendKeys(p)
            .Build()
            .Perform();

        testOutput.WriteLine("[LOG] Sent text to p element");

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
        wait.Until(d => {
            var e = d.FindElement(By.XPath($"//p[contains(text(),'{p}')]"));
            return e is not null;
        });

        testOutput.WriteLine("[LOG] Asserted p element's text content equal to text sent");
    }
}