using AzureServiceBusConsumer;
using Common;
using Elastic.Apm.NetCoreAll;
using Serilog;

var builder = Host.CreateDefaultBuilder(args)
    .UseAllElasticApm()
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    });

LoggingExtension.ConfigureLogging();

builder.UseSerilog();

var host = builder.Build();

await host.RunAsync();