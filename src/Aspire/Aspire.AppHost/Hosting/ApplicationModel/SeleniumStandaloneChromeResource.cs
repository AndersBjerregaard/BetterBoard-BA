namespace Aspire.Hosting.ApplicationModel;

public class SeleniumStandaloneChromeResource(string name, string? entrypoint = null) : ContainerResource(name, entrypoint), IResource
{
    internal const string? PrimaryEndpointName = "webdriver";
    internal const string Image = "selenium/standalone-chrome";
    internal const string Tag = "4.18.0-20240220";
}