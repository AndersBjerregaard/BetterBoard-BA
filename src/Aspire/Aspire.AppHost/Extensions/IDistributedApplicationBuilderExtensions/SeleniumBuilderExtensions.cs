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
            .WithImage(SeleniumStandaloneResource.Image, SeleniumStandaloneResource.Tag);
    }
}
