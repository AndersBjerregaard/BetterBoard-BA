using WebDriverXUnit.Abstractions;

namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface ICreateMeetingWindow {
    Result<string> FillAndConfirmMeeting(ref Xunit.Abstractions.ITestOutputHelper _testOutputHelper, ReadOnlySpan<char> browserName);
    void AssertMeetingConfirmed(string meetingTitle, ref Xunit.Abstractions.ITestOutputHelper _testOutputHelper, ReadOnlySpan<char> browserName);
}