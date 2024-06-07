namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface INavigationMenuWindow {
    /// <summary>
    /// Is assumed to generate a pop-up
    /// </summary>
    void CreateMeeting(ref Xunit.Abstractions.ITestOutputHelper _testOutputHelper, ReadOnlySpan<char> browserName);
    void AssertMeetingPopup(ref Xunit.Abstractions.ITestOutputHelper _testOutputHelper, ReadOnlySpan<char> browserName);
    void OpenCompanyDocuments(ReadOnlySpan<char> browserName);
}