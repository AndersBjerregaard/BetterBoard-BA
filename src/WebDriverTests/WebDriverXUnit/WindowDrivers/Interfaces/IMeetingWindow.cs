using WebDriverXUnit.Abstractions;

namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface IMeetingWindow {
    Result<string> FillAndConfirmMeeting(ref Xunit.Abstractions.ITestOutputHelper _testOutputHelper);
    void AssertMeetingConfirmed(string meetingTitle, ref Xunit.Abstractions.ITestOutputHelper _testOutputHelper);
}