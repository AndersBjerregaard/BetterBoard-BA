namespace Aspire.AppHost.ApplicationModel;

public class SeleniumEventBusResource(string name, string? entrypoint = null) : ContainerResource(name, entrypoint), IResource
{
    internal const string Image = "selenium/event-bus";
    internal const string Tag = "4.18.0-20240220";
}
