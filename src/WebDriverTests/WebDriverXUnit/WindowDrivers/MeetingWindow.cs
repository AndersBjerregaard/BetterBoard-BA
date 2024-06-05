using OpenQA.Selenium.Remote;
using WebDriverXUnit.WindowDrivers.Interfaces;
using Xunit.Abstractions;

namespace WebDriverXUnit.WindowDrivers;

public class MeetingWIndow(RemoteWebDriver driver, Uri baseUri, ITestOutputHelper testOutput) : IMeetingWindow
{
    public void AssertNavigation()
    {
        throw new NotImplementedException();
    }

    public void Navigate()
    {
        throw new NotImplementedException();
    }
}