namespace Aspire.Hosting.ApplicationModel;

public class SeleniumHubResource(string name, string? entrypoint = null) : ContainerResource(name, entrypoint), IResourceWithServiceDiscovery
{
    internal const string Image = "selenium/hub";
    internal const string Tag = "4.18.0-20240220";
    internal const string HttpEndpointName = "http";

    private EndpointReference? _endpointReference;

    public EndpointReference HttpEndpoint => _endpointReference ??= new(this, HttpEndpointName);

    public ReferenceExpression ConnectionStringExpression => ReferenceExpression.Create($"SE_EVENT_BUS_HOST={HttpEndpoint.Property(EndpointProperty.Host)}");
}