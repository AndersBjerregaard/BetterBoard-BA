using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace WebDriverXUnit.Helpers;

public static class AvailableDriverOptions {

    private static readonly DriverOptions[] DRIVER_OPTIONS = [new FirefoxOptions(), new ChromeOptions(), new EdgeOptions()];

    public static readonly DriverOptions[] CHROME_OPTIONS = [new ChromeOptions()];
    public static readonly DriverOptions[] EDGE_OPTIONS = [new EdgeOptions()];
    public static readonly DriverOptions[] FIREFOX_OPTIONS = [new FirefoxOptions()];

    public static DriverOptions[] Get() {
        return DRIVER_OPTIONS;
    }

    public static int GetAmount() {
        return DRIVER_OPTIONS.Length;
    }
}