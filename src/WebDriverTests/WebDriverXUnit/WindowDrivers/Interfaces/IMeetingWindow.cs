namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface IMeetingWindow : IWindowDriver {
    void FillAndConfirmMeeting();
    void AssertMeetingConfirmed();
}