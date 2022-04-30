using YukinoshitaBot.Extensions;
using YukinoshitaDemo;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        //services.AddHostedService<Worker>();
        services.AddYukinoshitaBot();
    })
    .Build();

await host.RunAsync();
