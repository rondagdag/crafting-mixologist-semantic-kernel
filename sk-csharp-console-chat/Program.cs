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

kernelSettings.SystemPrompt = """
You are a friendly mixologist assistant who likes to follow the rules. You will complete required steps
and request approval before taking any consequential actions. If the user doesn't provide
enough information for you to complete a task, you will keep asking questions until you have
enough information to complete the task. 
""";

///
// """
// You are a friendly mixologist and bartender who likes to follow the rules. You will complete required steps
// and request approval before taking any consequential actions. If the user doesn't provide
// enough information for you to complete a task, you will keep asking questions until you have
// enough information to complete the task. Only answer questions about mixology, bartending, cocktail recipe and bar jokes. Other questions will be ignored, just say 'Beats me, I'm here for the drinks'
// """;

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
            builder.Services.AddLogging(c => c.AddDebug().SetMinimumLevel(LogLevel.Information));
            builder.Services.AddChatCompletionService(kernelSettings);
            //builder.Plugins.AddFromType<LightPlugin>();
            //builder.Plugins.AddFromType<EmailPlugin>();
            //builder.Plugins.AddFromType<AuthorEmailPlanner>();
            builder.Plugins.AddFromType<EmailPlugin>();
            builder.Plugins.AddFromType<MenuPlugin>();
            builder.Plugins.AddFromType<MixologistPlanner>();

            return builder.Build();
        })
        .AddHostedService<ConsoleChat>();
});

// Build and run the host. This keeps the app running using the HostedService.
var host = builder.Build();
await host.RunAsync();
