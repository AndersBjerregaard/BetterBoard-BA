using System.Diagnostics;

namespace WebDriverXUnit.Fixtures;

#pragma warning disable S3881 // "IDisposable" should be implemented correctly
public class GridUri : IDisposable
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
{
    public Uri? WebDriverUri { get; private set; }

    public GridUri()
    {
        string? env = Environment.GetEnvironmentVariable("GRID_URI");

        if (string.IsNullOrWhiteSpace(env)) {
            Debug.WriteLine("Environment variable 'GRID_URI' was unset." + 
                " Defaulting Selenium Grid uri to localhost...");
            WebDriverUri = new Uri(@"http://localhost:4444");
        } else {
            Debug.WriteLine($"Environment variable 'GRID_URI' loaded as {env}");
            WebDriverUri = new Uri(env);
        }
    }

    void IDisposable.Dispose() => WebDriverUri = null;
}