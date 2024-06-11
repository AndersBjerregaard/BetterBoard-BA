using System.Diagnostics;
using System.Text.Json;
using OpenQA.Selenium.DevTools;
using WebDriverXUnit.Domain;
using WebDriverXUnit.Helpers;

namespace WebDriverXUnit.Fixtures;

public class TestVariables
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
            var settings = EnvironmentFileReader.Settings ??
                throw new NullReferenceException(nameof(EnvironmentFileReader.Settings));
            WebDriverUri = new Uri(settings.GridUri ?? throw new NullReferenceException(nameof(settings.GridUri)));
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
                var settings = EnvironmentFileReader.Settings ??
                    throw new NullReferenceException(nameof(EnvironmentFileReader.Settings));
                var uuid = settings.TestUuid ??
                    throw new NullReferenceException(nameof(settings.TestUuid));
                TestUuid = new UserCredentials(uuid + "@mail.dk", uuid);
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
            var settings = EnvironmentFileReader.Settings ??
                throw new NullReferenceException(nameof(EnvironmentFileReader.Settings));
            var uri = settings.TargetUri ??
                throw new NullReferenceException(nameof(settings.TargetUri));
            TargetUri = new Uri(uri);
            Debug.WriteLine($"'TARGET_URI' loaded as {TargetUri}");
        } else {
            Debug.WriteLine($"Environment variable 'TARGET_URI' loaded as {targetUri}");
            TargetUri = new Uri(targetUri);
        }
    }
}