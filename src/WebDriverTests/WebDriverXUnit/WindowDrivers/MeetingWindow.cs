using OpenQA.Selenium.Remote;
using WebDriverXUnit.WindowDrivers.Interfaces;

namespace WebDriverXUnit.WindowDrivers;

public class MeetingWindow(RemoteWebDriver driver, Uri baseUri) : IMeetingWindow
{
    public void AssertMeetingConfirmed(ref OpenQA.Selenium.DriverOptions options, ref Xunit.Abstractions.ITestOutputHelper _testOutputHelper)
    {
        _testOutputHelper.WriteLine("[INFO] Assert Meeting Confirmed");
    }

    public void AssertNavigation()
    {
        throw new NotImplementedException();
    }

    public void FillAndConfirmMeeting(ref Xunit.Abstractions.ITestOutputHelper _testOutputHelper)
    {
        _testOutputHelper.WriteLine("[INFO] Fill and Confirm Meeting.");
    }

    public void Navigate()
    {
        throw new NotImplementedException();
    }
}