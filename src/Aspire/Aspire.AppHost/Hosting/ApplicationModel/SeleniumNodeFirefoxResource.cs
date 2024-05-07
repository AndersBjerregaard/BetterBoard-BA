namespace Aspire.Hosting.ApplicationModel;

public class SeleniumNodeFirefoxResource(string name, string? entrypoint = null) : ContainerResource(name, entrypoint), IResource
{
    internal const string Image = "selenium/node-firefox";
    internal const string Tag = "4.18.0-20240220";
}