var builder = DistributedApplication.CreateBuilder(args);

builder.AddSeleniumHub();

builder.Build().Run();
