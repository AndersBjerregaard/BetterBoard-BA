namespace Aspire.AppHost.ApplicationModel;

public class SeleniumStandaloneResource(string name, string? entrypoint = null) : ContainerResource(name, entrypoint), IResource
{
    internal const string? PrimaryEndpointName = "webdriver";
    internal const string Image = "selenium/standalone-firefox";
    internal const string Tag = "4.18.0-20240220";
}