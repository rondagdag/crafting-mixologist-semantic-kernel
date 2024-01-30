using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Plugins;
using Planners;
// Load the kernel settings
var kernelSettings = KernelSettings.LoadSettings();

// Create the host builder with logging configured from the kernel settings.
var builder = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.SetMinimumLevel(kernelSettings.LogLevel ?? LogLevel.Warning);
    });

// Configure the services for the host
builder.ConfigureServices((context, services) =>
{
    
    // Add kernel settings to the host builder
    services
        .AddSingleton<KernelSettings>(kernelSettings)
        .AddTransient<Kernel>(serviceProvider => {
            var builder = Kernel.CreateBuilder();
            builder.Services.AddLogging(c => c.AddDebug().SetMinimumLevel(LogLevel.Debug));
            builder.Services.AddChatCompletionService(kernelSettings);
            //builder.Plugins.AddFromType<LightPlugin>();
            builder.Plugins.AddFromType<EmailPlugin>();
            //builder.Plugins.AddFromType<AuthorEmailPlanner>();
            builder.Plugins.AddFromType<MixologistPlanner>();
            builder.Plugins.AddFromType<MenuPlugin>();

            return builder.Build();
        })
        .AddHostedService<ConsoleChat>();
});

// Build and run the host. This keeps the app running using the HostedService.
var host = builder.Build();
await host.RunAsync();
