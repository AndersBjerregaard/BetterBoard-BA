using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace WebDriverXUnit.Extensions;

public static class DriverOptionsExtensions {
    public static DriverOptions ApplyOptionsArguments(this DriverOptions options, string[] arguments) {
        var typeName = options.GetType().Name;
        if (typeName == nameof(ChromeOptions))
        {
            var chromeOptions = options as ChromeOptions;
            // Flags seems to work on all browsers https://github.com/GoogleChrome/chrome-launcher/blob/main/docs/chrome-flags-for-tools.md
            chromeOptions?.AddArguments(arguments);
        }
        else if (typeName == nameof(FirefoxOptions))
        {
            var firefoxOptions = options as FirefoxOptions;
            firefoxOptions?.AddArguments(arguments);
        }
        else if (typeName == nameof(EdgeOptions))
        {
            var edgeOptions = options as EdgeOptions;
            edgeOptions?.AddArguments("--no-sandbox", "--disable-dev-shm-usage", "--guest", "--headless", "--window-size=1920,1080"); // Guest argument to disable personalized edge pop-ups
        }
        return options;
    }
}