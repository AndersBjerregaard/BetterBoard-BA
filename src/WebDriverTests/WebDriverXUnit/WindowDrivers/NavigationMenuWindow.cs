using OpenQA.Selenium.Remote;
using WebDriverXUnit.WindowDrivers.Interfaces;

namespace WebDriverXUnit.WindowDrivers;

public class NavigationMenuWindow(RemoteWebDriver driver, Uri baseUri) : INavigationMenuWindow
{
    public void AssertConfirmedMeeting()
    {
        throw new NotImplementedException();
    }

    public void AssertMeetingPopup()
    {
        throw new NotImplementedException();
    }

    public void AssertNavigation()
    {
        throw new NotImplementedException();
    }

    public void CreateMeeting()
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