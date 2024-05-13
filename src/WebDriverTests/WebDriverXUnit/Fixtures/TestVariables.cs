using System.Diagnostics;
using OpenQA.Selenium.DevTools;
using WebDriverXUnit.Helpers;

namespace WebDriverXUnit.Fixtures;

#pragma warning disable S3881 // "IDisposable" should be implemented correctly
public class TestVariables : IDisposable
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
{
    public Uri? WebDriverUri { get; private set; }
    public string? TestUuid { get; private set; }
    public Uri? TargetUri { get; private set; }

    public TestVariables()
    {
        string? gridUri = Environment.GetEnvironmentVariable("GRID_URI");

        if (string.IsNullOrWhiteSpace(gridUri)) {
            Debug.WriteLine("Environment variable 'GRID_URI' was unset." + 
                " Defaulting to environment file...");
            WebDriverUri = new Uri(EnvironmentFileReader.Settings.GridUri);
            Debug.WriteLine($"'GRID_URI' loaded as {WebDriverUri}");
        } else {
            Debug.WriteLine($"Environment variable 'GRID_URI' loaded as {gridUri}");
            WebDriverUri = new Uri(gridUri);
        }

        string? testUuid = Environment.GetEnvironmentVariable("TEST_UUID");

        if (string.IsNullOrWhiteSpace(testUuid)) {
            Debug.WriteLine("Environment variable 'TEST_UUID' was unset." + 
                " Defaulting to environment file...");
            TestUuid = EnvironmentFileReader.Settings.TestUuid;
            Debug.WriteLine($"'TEST_UUID' loaded as {TestUuid}");
        } else {
            Debug.WriteLine($"Environment variable 'TEST_UUID' loaded as {testUuid}");
            TestUuid = testUuid;
        }

        string? targetUri = Environment.GetEnvironmentVariable("TARGET_URI");

        if (string.IsNullOrWhiteSpace(targetUri)) {
            Debug.WriteLine("Environment variable 'TARGET_URI' was unset." + 
                " Defaulting to environment file...");
            TargetUri = new Uri(EnvironmentFileReader.Settings.TargetUri);
            Debug.WriteLine($"'TARGET_URI' loaded as {TargetUri}");
        } else {
            Debug.WriteLine($"Environment variable 'TARGET_URI' loaded as {targetUri}");
            TestUuid = targetUri;
        }
    }

    void IDisposable.Dispose() => WebDriverUri = null;
}