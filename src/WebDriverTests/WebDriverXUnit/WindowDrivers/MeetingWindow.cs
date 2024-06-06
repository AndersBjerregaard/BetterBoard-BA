using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers.Interfaces;
using Xunit.Abstractions;

namespace WebDriverXUnit.WindowDrivers;

public class MeetingWIndow(RemoteWebDriver driver, ITestOutputHelper testOutput) : IMeetingWindow
{
    public void AssertCurrentViewedMeeting(string meetingTitle)
    {
        var title = new WebElementFinder(driver).Find(By.XPath("//span[@id='meetingTitle']"));
        Assert.NotNull(title);
        Assert.Equal(meetingTitle, title.Text);
    }

    public IWebElement GetMeetingAgendaSection()
    {
        var section = new WebElementFinder(driver).Find(By.XPath("//h5[text()='AGENDA ']/../.."));
        Assert.NotNull(section);
        Assert.Equal("section", section.TagName);
        var header = section.FindElement(By.TagName("h5"));
        Assert.NotNull(header);
        Assert.Equal("agenda", header.Text.ToLower());
        return section;
    }

    public IWebElement GetSummaryOfFirstAgendaItem(IWebElement meetingSection) {
        // Relies on the existence of at least one agenda item
        var collapse = meetingSection.FindElement(By.XPath(".//button[@class='btn btn-sm btn-primary' and @data-toggle='collapse']"));
        Assert.NotNull(collapse);
        collapse.Click();

        testOutput.WriteLine("[LOG] Clicked collapse");

        Thread.Sleep(TimeSpan.FromMilliseconds(500));

        var showSummary = meetingSection.FindElement(By.XPath(".//button[contains(text(),'Show summary')]"));
        showSummary.Click();

        testOutput.WriteLine("[LOG] Clicked summary show");

        var tinymceSection = new WebElementFinder(driver).FindMultiple(By.XPath("//div[@class='tox-edit-area']/descendant-or-self::*"));
        Assert.NotNull(tinymceSection);
        IWebElement? iframe = null;
        foreach (var item in tinymceSection)
        {
            if (item.TagName == "iframe") {
                iframe = item;
                break;
            }
        }

        Assert.NotNull(iframe);

        testOutput.WriteLine("[LOG] Asserted tinymce editor");

        return iframe;
    }

    public void SaveSummary(IWebElement agendaSection)
    {
        var saveBtn = agendaSection.FindElement(By.XPath(".//button[contains(text(),'Save')]"));
        Assert.NotNull(saveBtn);
        saveBtn.Click();

        testOutput.WriteLine("[LOG] Saved summary");
    }

    public void UploadDocumentToFirstAgendaItem(IWebElement agendaSection)
    {
        // Relies on the existence of at least one agenda item
        var collapse = agendaSection.FindElement(By.XPath(".//button[@class='btn btn-sm btn-primary' and @data-toggle='collapse']"));
        Assert.NotNull(collapse);
        collapse.Click();

        testOutput.WriteLine("[LOG] Clicked collapse");

        var uploadBtn = agendaSection.FindElement(By.XPath(".//button[contains(text(),'Upload documents')]"));
        Assert.NotNull(uploadBtn);
        Assert.True(uploadBtn.Displayed);
        uploadBtn.Click();

        testOutput.WriteLine("[LOG] Clicked 'Upload documents'");

        var uploadForm = agendaSection.FindElement(By.XPath(".//span[contains(text(),'upload')]/../../."));
        Assert.NotNull(uploadForm);
        Assert.True(uploadForm.Displayed);
        uploadForm.Click();

        testOutput.WriteLine("[WARNING] No implementation for file upload");

        testOutput.WriteLine("[LOG] Asserted upload form");
    }
}