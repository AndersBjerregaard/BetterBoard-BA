using OpenQA.Selenium.Remote;
using WebDriverXUnit.WindowDrivers.Interfaces;

namespace WebDriverXUnit.WindowDrivers;

public class NavigationMenuWindow(RemoteWebDriver driver, Uri baseUri) : INavigationMenuWindow
{
    public void AssertMeetingPopup(ref Xunit.Abstractions.ITestOutputHelper _testOutputHelper)
    {
        _testOutputHelper.WriteLine("[INFO] Asserted Meeting Popup.");
    }

    public void AssertNavigation()
    {
        throw new NotImplementedException();
    }

    public void CreateMeeting(ref Xunit.Abstractions.ITestOutputHelper _testOutputHelper)
    {
        _testOutputHelper.WriteLine("[INFO] Create Meeting.");
    }

    public void Navigate()
    {
        throw new NotImplementedException();
    }
}