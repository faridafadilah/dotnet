using ContosoWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<LogTimes>();
    })
    .Build();

await host.RunAsync();
