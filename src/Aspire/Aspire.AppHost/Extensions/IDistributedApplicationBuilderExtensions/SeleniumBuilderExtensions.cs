using Aspire.AppHost.ApplicationModel;

public static class SeleniumBuilderExtensions
{
    public static IResourceBuilder<SeleniumStandaloneResource> AddSeleniumStandalone(this IDistributedApplicationBuilder builder,
            string name,
            int port)
    {
        var seleniumStandalone = new SeleniumStandaloneResource(name);

        return builder.AddResource(seleniumStandalone)
            .WithHttpEndpoint(port: port, targetPort: 4444, name: SeleniumStandaloneResource.PrimaryEndpointName) // Internal port defaults to 4444
            .WithExternalHttpEndpoints() // Allow external ingress communication outside container environment
            .WithImage(SeleniumStandaloneResource.Image, SeleniumStandaloneResource.Tag);
    }

    public static IDistributedApplicationBuilder AddSeleniumGrid(this IDistributedApplicationBuilder builder)
    {
        return builder;
    }
}
