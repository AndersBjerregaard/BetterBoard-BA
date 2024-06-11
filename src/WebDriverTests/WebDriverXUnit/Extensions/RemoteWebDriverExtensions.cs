using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using Xunit.Abstractions;

namespace WebDriverXUnit.Extensions;

public static class RemoteWebDriverExtensions {
    public static RemoteWebDriver Dump(this RemoteWebDriver driver, DriverOptions options, ITestOutputHelper testOutput) {
        string path = $"./dump/";
        var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + path);

        testOutput.WriteLine($"[LOG] Dumping data to: '{fullPath}'");

        var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var optionsType = options.GetType();

        if (optionsType == typeof(ChromeOptions) || optionsType == typeof(EdgeOptions)) {
            if ((options as ChromiumOptions ?? throw new NullReferenceException(nameof(options))).Arguments.Contains("--headless"))
            {
                var scrShot = driver?.GetScreenshot();
                scrShot?.SaveAsFile(path + $"{options.BrowserName}-{time}.png");
                testOutput.WriteLine("[LOG] Saved screenshot");
            }
        } else if (optionsType == typeof(EdgeOptions) && (options as EdgeOptions ?? throw new NullReferenceException(nameof(options))).Arguments.Contains("--headless"))
        {
            var scrShot = driver?.GetScreenshot();
            scrShot?.SaveAsFile(path + $"{options.BrowserName}-{time}.png");
            testOutput.WriteLine("[LOG] Saved screenshot");
        }

        using (StreamWriter outputFile = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + path + $"{options.BrowserName}-{time}.html"))) {
            var html = driver?.PageSource;
            outputFile.WriteLine(html);
        }
        testOutput.WriteLine("[LOG] Dumped HTML");
        return driver ?? throw new NullReferenceException(nameof(driver));
    }
}