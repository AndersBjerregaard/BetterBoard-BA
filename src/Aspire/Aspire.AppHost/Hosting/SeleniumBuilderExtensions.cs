using Aspire.Hosting.ApplicationModel;

namespace Aspire.Hosting;

public static class SeleniumBuilderExtensions
{
    public static IResourceBuilder<SeleniumStandaloneChromeResource> AddSeleniumStandaloneChrome(this IDistributedApplicationBuilder builder,
            string name,
            int port)
    {
        var seleniumStandalone = new SeleniumStandaloneChromeResource(name);

        return builder.AddResource(seleniumStandalone)
            .WithEnvironment("SE_NODE_SESSION_TIMEOUT", "60")
            .WithEnvironment("SE_NODE_OVERRIDE_MAX_SESSIONS", "true")
            .WithEnvironment("SE_NODE_MAX_SESSIONS", "3")
            .WithHttpEndpoint(port: port, targetPort: 4444, name: SeleniumStandaloneChromeResource.PrimaryEndpointName) // Internal port defaults to 4444
            .WithExternalHttpEndpoints() // Allow external ingress communication outside container environment
            .WithImage(SeleniumStandaloneChromeResource.Image, SeleniumStandaloneChromeResource.Tag);
    }

    public static IResourceBuilder<SeleniumStandaloneEdgeResource> AddSeleniumStandaloneEdge(this IDistributedApplicationBuilder builder,
            string name,
            int port)
    {
        var seleniumStandalone = new SeleniumStandaloneEdgeResource(name);

        return builder.AddResource(seleniumStandalone)
            .WithEnvironment("SE_NODE_SESSION_TIMEOUT", "60")
            .WithEnvironment("SE_NODE_OVERRIDE_MAX_SESSIONS", "true")
            .WithEnvironment("SE_NODE_MAX_SESSIONS", "3")
            .WithHttpEndpoint(port: port, targetPort: 4444, name: SeleniumStandaloneEdgeResource.PrimaryEndpointName) // Internal port defaults to 4444
            .WithExternalHttpEndpoints() // Allow external ingress communication outside container environment
            .WithImage(SeleniumStandaloneEdgeResource.Image, SeleniumStandaloneEdgeResource.Tag);
    }

    public static IResourceBuilder<SeleniumStandaloneFirefoxResource> AddSeleniumStandaloneFirefox(this IDistributedApplicationBuilder builder,
            string name,
            int port)
    {
        var seleniumStandalone = new SeleniumStandaloneFirefoxResource(name);

        return builder.AddResource(seleniumStandalone)
            .WithEnvironment("SE_NODE_SESSION_TIMEOUT", "60")
            .WithEnvironment("SE_NODE_OVERRIDE_MAX_SESSIONS", "true")
            .WithEnvironment("SE_NODE_MAX_SESSIONS", "3")
            .WithHttpEndpoint(port: port, targetPort: 4444, name: SeleniumStandaloneFirefoxResource.PrimaryEndpointName) // Internal port defaults to 4444
            .WithExternalHttpEndpoints() // Allow external ingress communication outside container environment
            .WithImage(SeleniumStandaloneFirefoxResource.Image, SeleniumStandaloneFirefoxResource.Tag);
    }

    public static IDistributedApplicationBuilder AddSeleniumGrid(this IDistributedApplicationBuilder builder)
    {
        var seleniumEventBus = new SeleniumEventBusResource("selenium-event-bus");

        builder.AddResource<SeleniumEventBusResource>(seleniumEventBus)
            .WithEndpoint(4442, 4442, "tcp" ,"publish")
            .WithEndpoint(4443, 4443, "tcp" ,"subscribe")
            .WithEndpoint(5557, 5557, "tcp" ,"logs");

        var seleniumSessions = new SeleniumSessionsResource("selenium-sessions");

        builder.AddResource<SeleniumSessionsResource>(seleniumSessions)
            .WithEndpoint(5556, 5556, "tcp" ,"map")
            .WithEnvironment("SE_EVENT_BUS_HOST", seleniumEventBus.Name) // May have to use builder.WithReference() method instead
            .WithEnvironment("SE_EVENT_BUS_PUBLISH_PORT", "4442")
            .WithEnvironment("SE_EVENT_BUS_SUBSCRIBE_PORT", "4443");

        return builder;
    }

    public static IDistributedApplicationBuilder AddSeleniumHub(this  IDistributedApplicationBuilder builder)
    {
        var seleniumHub = new SeleniumHubResource("selenium-hub");

        var resource = builder.AddResource<SeleniumHubResource>(seleniumHub)
            .WithEndpoint(4442, 4442, "tcp", "publish")
            .WithEndpoint(4443, 4443, "tcp", "subscribe")
            .WithHttpEndpoint(4444, 4444, "webdriver")
            .WithExternalHttpEndpoints()
            .WithImage(SeleniumHubResource.Image, SeleniumHubResource.Tag);

        var seleniumNodeChrome = new SeleniumNodeChromeResource("node-chrome");

        builder.AddResource(seleniumNodeChrome)
            .WithReference(resource)
            .WithEnvironment("SE_EVENT_BUS_HOST", seleniumHub.Name)
            .WithEnvironment("SE_EVENT_BUS_PUBLISH_PORT", "4442")
            .WithEnvironment("SE_EVENT_BUS_SUBSCRIBE_PORT", "4443")
            .WithEnvironment("SE_NODE_SESSION_TIMEOUT", "60")
            .WithEnvironment("SE_NODE_OVERRIDE_MAX_SESSIONS", "true")
            .WithEnvironment("SE_NODE_MAX_SESSIONS", "3")
            .WithImage(SeleniumNodeChromeResource.Image, SeleniumNodeChromeResource.Tag);

        var seleniumNodeEdge = new SeleniumNodeEdgeResource("node-edge");

        builder.AddResource(seleniumNodeEdge)
            .WithReference(resource)
            .WithEnvironment("SE_EVENT_BUS_HOST", seleniumHub.Name)
            .WithEnvironment("SE_EVENT_BUS_PUBLISH_PORT", "4442")
            .WithEnvironment("SE_EVENT_BUS_SUBSCRIBE_PORT", "4443")
            .WithEnvironment("SE_NODE_SESSION_TIMEOUT", "60")
            .WithEnvironment("SE_NODE_OVERRIDE_MAX_SESSIONS", "true")
            .WithEnvironment("SE_NODE_MAX_SESSIONS", "3")
            .WithImage(SeleniumNodeEdgeResource.Image, SeleniumNodeEdgeResource.Tag);

        var seleniumNodeFirefox = new SeleniumNodeFirefoxResource("node-firefox");

        builder.AddResource(seleniumNodeFirefox)
            .WithReference(resource)
            .WithEnvironment("SE_EVENT_BUS_HOST", seleniumHub.Name)
            .WithEnvironment("SE_EVENT_BUS_PUBLISH_PORT", "4442")
            .WithEnvironment("SE_EVENT_BUS_SUBSCRIBE_PORT", "4443")
            .WithEnvironment("SE_NODE_SESSION_TIMEOUT", "60")
            .WithEnvironment("SE_NODE_OVERRIDE_MAX_SESSIONS", "true")
            .WithEnvironment("SE_NODE_MAX_SESSIONS", "3")
            .WithImage(SeleniumNodeFirefoxResource.Image, SeleniumNodeFirefoxResource.Tag);

        return builder;
    }
}
