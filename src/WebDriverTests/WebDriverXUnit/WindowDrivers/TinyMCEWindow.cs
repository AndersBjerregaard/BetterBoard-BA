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
        var paragraphs = new WebElementFinder(driver).FindMultiple(By.TagName("p"));
        Assert.NotNull(paragraphs);
        Assert.True(paragraphs.Any());
        var editableParas = paragraphs.Where(p => p.GetDomProperty("isContentEditable") == "True");

        foreach (var para in editableParas) {
            para.Clear();
        }

        testOutput.WriteLine("[LOG] Found editable tinymce p element");

        new Actions(driver)
            .Click(editableParas.First())
            .SendKeys(p)
            .Build()
            .Perform();

        testOutput.WriteLine("[LOG] Sent text to p element");

        var substrings = p.Split(' ');
        foreach (var s in substrings)
        {
            Assert.Contains(s, driver.PageSource);
        }

        testOutput.WriteLine("[LOG] Asserted p element's text content equal to text sent");
    }
}