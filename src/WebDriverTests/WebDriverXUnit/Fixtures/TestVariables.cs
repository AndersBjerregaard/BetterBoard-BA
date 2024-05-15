using System.Diagnostics;
using System.Text.Json;
using OpenQA.Selenium.DevTools;
using WebDriverXUnit.Domain;
using WebDriverXUnit.Helpers;

namespace WebDriverXUnit.Fixtures;

public class TestVariables : IDisposable
{
    public Uri? WebDriverUri { get; private set; }
    public UserCredentials? TestUuid { get; private set; }
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

        string? decoratedCreds = Environment.GetEnvironmentVariable("TEST_CREDS");

        if (!string.IsNullOrWhiteSpace(decoratedCreds))
        {
            Debug.WriteLine("Value for environment variable 'TEST_CREDS' detected, omitting 'TEST_UUID'...");
            UserCredentials? deserialized = JsonSerializer.Deserialize<UserCredentials>(decoratedCreds);
            TestUuid = deserialized;
        }
        else
        {
            string? testUuid = Environment.GetEnvironmentVariable("TEST_UUID");

            if (string.IsNullOrWhiteSpace(testUuid))
            {
                Debug.WriteLine("Environment variable 'TEST_UUID' was unset." +
                    " Defaulting to environment file...");
                TestUuid = new UserCredentials(EnvironmentFileReader.Settings.TestUuid + "@mail.dk", EnvironmentFileReader.Settings.TestUuid);
                Debug.WriteLine($"'TEST_UUID' loaded as {TestUuid}");
            }
            else
            {
                Debug.WriteLine($"Environment variable 'TEST_UUID' loaded as {testUuid}");
                TestUuid = new UserCredentials(testUuid + "@mail.dk", testUuid);
            }
        }



        string? targetUri = Environment.GetEnvironmentVariable("TARGET_URI");

        if (string.IsNullOrWhiteSpace(targetUri)) {
            Debug.WriteLine("Environment variable 'TARGET_URI' was unset." + 
                " Defaulting to environment file...");
            TargetUri = new Uri(EnvironmentFileReader.Settings.TargetUri);
            Debug.WriteLine($"'TARGET_URI' loaded as {TargetUri}");
        } else {
            Debug.WriteLine($"Environment variable 'TARGET_URI' loaded as {targetUri}");
            TargetUri = new Uri(targetUri);
        }
    }

    void IDisposable.Dispose() {
        WebDriverUri = null;
        TestUuid = null;
        TargetUri = null;
    }
}