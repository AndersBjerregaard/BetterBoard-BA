using System.Diagnostics;
using System.Text.Json;
using WebDriverXUnit.Domain;
using WebDriverXUnit.Helpers;

namespace WebDriverXUnit.Factories;

public static class TestVariablesFactory
{
    private static Uri? _seleniumGridUri;
    private static Uri? _sutUri;
    private static UserCredentials? _userCredentials;
    private static Guid _testUuid = Guid.Empty;
    private static string? _shortId;
    public static readonly string[] DEFAULT_WEBDRIVER_ARGUMENTS = ["--no-sandbox", "--disable-dev-shm-usage", "--incognito", "--window-size=1920,1080", "--headless"];

    public static Uri GetSeleniumGridUri()
    {
        if (_seleniumGridUri is not null) {
            return _seleniumGridUri;
        }

        string? gridUri = Environment.GetEnvironmentVariable("GRID_URI");

        if (string.IsNullOrWhiteSpace(gridUri)) {
            Debug.WriteLine("Environment variable 'GRID_URI' was unset." + 
                " Defaulting to environment file...");
            var settings = EnvironmentFileReader.Settings ??
                throw new NullReferenceException(nameof(EnvironmentFileReader.Settings));
            _seleniumGridUri = new Uri(settings.GridUri ?? throw new NullReferenceException(nameof(settings.GridUri)));
            Debug.WriteLine($"'GRID_URI' loaded as {_seleniumGridUri}");
        } else {
            Debug.WriteLine($"Environment variable 'GRID_URI' loaded as {gridUri}");
            _seleniumGridUri = new Uri(gridUri);
        }
        return _seleniumGridUri;
    }

    public static Uri GetSutUri()
    {
        if (_sutUri is not null) {
            return _sutUri;
        }

        string? targetUri = Environment.GetEnvironmentVariable("TARGET_URI");

        if (string.IsNullOrWhiteSpace(targetUri)) {
            Debug.WriteLine("Environment variable 'TARGET_URI' was unset." + 
                " Defaulting to environment file...");
            var settings = EnvironmentFileReader.Settings ??
                throw new NullReferenceException(nameof(EnvironmentFileReader.Settings));
            var uri = settings.TargetUri ??
                throw new NullReferenceException(nameof(settings.TargetUri));
            _sutUri = new Uri(uri);
            Debug.WriteLine($"'TARGET_URI' loaded as {_sutUri}");
        } else {
            Debug.WriteLine($"Environment variable 'TARGET_URI' loaded as {targetUri}");
            _sutUri = new Uri(targetUri);
        }
        return _sutUri;
    }

    public static UserCredentials GetUserCredentials()
    {
        if (_userCredentials is not null) {
            return _userCredentials;
        }

        string? decoratedCreds = Environment.GetEnvironmentVariable("TEST_CREDS");

        if (!string.IsNullOrWhiteSpace(decoratedCreds))
        {
            Debug.WriteLine("Value for environment variable 'TEST_CREDS' detected, omitting 'TEST_UUID'...");
            UserCredentials? deserialized = JsonSerializer.Deserialize<UserCredentials>(decoratedCreds);
            _userCredentials = deserialized ?? throw new JsonException($"Could not serialize to {nameof(UserCredentials)}");
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
                _userCredentials = new UserCredentials(uuid + "@mail.dk", uuid);
                Debug.WriteLine($"'TEST_UUID' loaded as {_userCredentials}");
            }
            else
            {
                Debug.WriteLine($"Environment variable 'TEST_UUID' loaded as {testUuid}");
                _userCredentials = new UserCredentials(testUuid + "@mail.dk", testUuid);
            }
        }
        return _userCredentials;
    }

    public static Guid GetTestUuid() {
        if (_testUuid != Guid.Empty) {
            return _testUuid;
        }

        string? testUuid = Environment.GetEnvironmentVariable("TEST_UUID");

        if (string.IsNullOrWhiteSpace(testUuid)) {
            throw new NullReferenceException(nameof(testUuid));
        }

        _testUuid = Guid.Parse(testUuid);

        return _testUuid;
    }

    public static string GetShortId() {
        if (_shortId is not null) {
            return _shortId;
        }
        if (_testUuid != Guid.Empty) {
            _shortId = _testUuid.ToString("N")[^4..];
            return _shortId;
        }
        string? testUuid = Environment.GetEnvironmentVariable("TEST_UUID");

        if (string.IsNullOrWhiteSpace(testUuid)) {
            throw new NullReferenceException(nameof(testUuid));
        }

        _testUuid = Guid.Parse(testUuid);

        _shortId = _testUuid.ToString("N")[^4..];
        return _shortId;
    }
}