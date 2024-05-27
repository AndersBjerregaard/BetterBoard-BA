using OpenQA.Selenium.Remote;
using WebDriverXUnit.WindowDrivers.Interfaces;

namespace WebDriverXUnit.WindowDrivers;

public class MeetingWindow(RemoteWebDriver driver, Uri baseUri) : IMeetingWindow
{
    public void AssertMeetingConfirmed()
    {
        throw new NotImplementedException();
    }

    public void AssertNavigation()
    {
        throw new NotImplementedException();
    }

    public void FillAndConfirmMeeting()
    {
        throw new NotImplementedException();
    }

    public void Navigate()
    {
        throw new NotImplementedException();
    }
}