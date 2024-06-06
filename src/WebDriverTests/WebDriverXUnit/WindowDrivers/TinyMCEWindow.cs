using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
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
        
        // Remove existing text
        if (!string.IsNullOrWhiteSpace(paragraph.Text)) {
            new Actions(driver)
                .Click(paragraph)
                .KeyDown(Keys.Control)
                .SendKeys("a")
                .KeyUp(Keys.Control)
                .SendKeys(Keys.Backspace)
                .Build()
                .Perform();
            testOutput.WriteLine("[LOG] Removed existing text");
            paragraph = new WebElementFinder(driver).Find(By.TagName("p"));
        }

        new Actions(driver)
            .Click(paragraph)
            .SendKeys(p)
            .Build()
            .Perform();

        testOutput.WriteLine("[LOG] Sent text to p element");

        paragraph = new WebElementFinder(driver).Find(By.TagName("p"));

        Assert.NotNull(paragraph);
        Assert.Equal(p, paragraph.Text);

        testOutput.WriteLine("[LOG] Asserted p element's text content equal to text sent");
    }
}