namespace Aspire.AppHost.ApplicationModel;

public class SeleniumDistributorResource(string name, string? entrypoint = null) : ContainerResource(name, entrypoint), IResource
{
    internal const string Image = "selenium/distributor";
    internal const string Tag = "4.18.0-20240220";
}
