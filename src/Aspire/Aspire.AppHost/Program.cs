var builder = DistributedApplication.CreateBuilder(args);

var customContainer = builder.AddSeleniumStandalone("namnam", 8082);

builder.Build().Run();
