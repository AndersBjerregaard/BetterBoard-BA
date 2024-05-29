namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface IMeetingWindow : IWindowDriver {
    void FillAndConfirmMeeting(ref Xunit.Abstractions.ITestOutputHelper _testOutputHelper);
    void AssertMeetingConfirmed(ref OpenQA.Selenium.DriverOptions options, ref Xunit.Abstractions.ITestOutputHelper _testOutputHelper);
}