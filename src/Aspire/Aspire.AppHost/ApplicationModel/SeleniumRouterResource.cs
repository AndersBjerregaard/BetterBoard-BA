namespace Aspire.AppHost.ApplicationModel;

public class SeleniumRouterResource(string name, string? entrypoint = null) : ContainerResource(name, entrypoint), IResource
{
    internal const string Image = "selenium/router";
    internal const string Tag = "4.18.0-20240220";
}
