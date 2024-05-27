namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface INavigationMenuWindow : IWindowDriver {
    /// <summary>
    /// Is assumed to generate a pop-up
    /// </summary>
    void CreateMeeting();
    void AssertMeetingPopup();
    void FillAndConfirmMeeting();
    void AssertConfirmedMeeting();
}