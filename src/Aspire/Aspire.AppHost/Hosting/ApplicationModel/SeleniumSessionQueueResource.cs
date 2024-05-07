namespace Aspire.Hosting.ApplicationModel;

public class SeleniumSessionQueueResource(string name, string? entrypoint = null) : ContainerResource(name, entrypoint), IResource
{
    internal const string Image = "selenium/session-queue";
    internal const string Tag = "4.18.0-20240220";
}
