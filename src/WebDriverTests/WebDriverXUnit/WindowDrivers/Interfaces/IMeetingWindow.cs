using OpenQA.Selenium;

namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface IMeetingWindow {
    void AssertCurrentViewedMeeting(string meetingTitle);
    IWebElement GetMeetingAgendaSection();
    void UploadDocumentToFirstAgendaItem(IWebElement agendaSection);
    IWebElement GetSummaryOfFirstAgendaItem(IWebElement meetingSection);
    void SaveSummary(IWebElement agendaSection);
}