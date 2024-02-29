using System.Diagnostics;

namespace WebDriverXUnit.Fixtures;

public class GridUri : IDisposable
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

    public void Dispose() => WebDriverUri = null;
}