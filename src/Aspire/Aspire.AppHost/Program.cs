var builder = DistributedApplication.CreateBuilder(args);

var customContainer = builder.AddSeleniumStandalone("selenium-standalone", 8082);

builder.Build().Run();
