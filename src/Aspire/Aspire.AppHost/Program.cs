var builder = DistributedApplication.CreateBuilder(args);

builder.AddSeleniumStandaloneChrome("standalone-chrome", 4444);

builder.AddSeleniumStandaloneEdge("standalone-edge", 4445);

builder.AddSeleniumStandaloneFirefox("standalone-firefox", 4446);

builder.Build().Run();
