using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers.Interfaces;
using Xunit.Abstractions;

namespace WebDriverXUnit.WindowDrivers;

public class MeetingWIndow(RemoteWebDriver driver, Uri baseUri, ITestOutputHelper testOutput) : IMeetingWindow
{
    public void AssertCurrentViewedMeeting(string meetingTitle)
    {
        var title = new IWebElementFinder(driver).Find(By.XPath("//span[@id='meetingTitle']"));
        Assert.NotNull(title);
        Assert.Equal(meetingTitle, title.Text);
    }

    public IWebElement GetMeetingAgendaSection()
    {
        var section = new IWebElementFinder(driver).Find(By.XPath("//h5[text()='AGENDA ']/../.."));
        Assert.NotNull(section);
        Assert.Equal("section", section.TagName);
        var header = section.FindElement(By.TagName("h5"));
        Assert.NotNull(header);
        Assert.Equal("agenda", header.Text.ToLower());
        return section;
    }

    public void UploadDocumentToFirstAgendaItem(IWebElement meetingSection)
    {
        var docCollapse = meetingSection.FindElement(By.XPath(".//button[@class='btn btn-sm btn-primary' and @data-toggle='collapse']"));
        Assert.NotNull(docCollapse);
        docCollapse.Click();

        testOutput.WriteLine("[LOG] Clicked documents collapse");

        var uploadBtn = meetingSection.FindElement(By.XPath(".//button[contains(text(),'Upload documents')]"));
        Assert.NotNull(uploadBtn);
        Assert.True(uploadBtn.Displayed);
        uploadBtn.Click();

        testOutput.WriteLine("[LOG] Clicked 'Upload documents'");

        var uploadForm = meetingSection.FindElement(By.XPath(".//span[contains(text(),'upload')]/../../."));
        Assert.NotNull(uploadForm);
        Assert.True(uploadForm.Displayed);
        uploadForm.Click();

        testOutput.WriteLine("[WARNING] No implementation for file upload");

        testOutput.WriteLine("[LOG] Asserted upload form");
    }
}