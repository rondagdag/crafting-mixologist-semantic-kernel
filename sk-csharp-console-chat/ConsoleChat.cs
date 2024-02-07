using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

/// <summary>
/// This is the main application service.
/// This takes console input, then sends it to the configured AI service, and then prints the response.
/// All conversation history is maintained in the chat history.
/// </summary>
internal class ConsoleChat : IHostedService
{
    private readonly Kernel _kernel;
    private readonly IHostApplicationLifetime _lifeTime;

    public ConsoleChat(Kernel kernel, IHostApplicationLifetime lifeTime)
    {
        this._kernel = kernel;
        this._lifeTime = lifeTime;
    }

    /// <summary>
    /// Start the service.
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => this.ExecuteAsync(cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Stop a running service.
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary>
    /// The main execution loop. It will use any of the available plugins to perform actions
    /// </summary>
    private async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        ChatHistory chatHistory = new ChatHistory("""
        I'm your AI Copilot bartender, limit topic of conversation to mingling, cocktails, mixology, bar jokes, sending email suggestions. I can only give explicit instructions or say 'Beats me, I'm just here for the drinks' if it's outside topic.
        I always seek approval before taking any action. If details are lacking, I'll ask questions until I can complete the task. I will ask for clarification if its needed. I will not do anything without approval.
        """);
        IChatCompletionService chatCompletionService = this._kernel.GetRequiredService<IChatCompletionService>();

        // Loop till we are cancelled
        while (!cancellationToken.IsCancellationRequested)
        {
            // Get user input
            System.Console.Write("User > ");
            var request = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(request))
            {
                continue;
            }
            chatHistory.AddUserMessage(request!);

            // Get the chat completions
            
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
            };
            IAsyncEnumerable<StreamingChatMessageContent> result =
                chatCompletionService.GetStreamingChatMessageContentsAsync(
                    chatHistory,
                    executionSettings: openAIPromptExecutionSettings,
                    kernel: this._kernel,
                    cancellationToken: cancellationToken);

            // Print the chat completions
            ChatMessageContent? chatMessageContent = null;
            await foreach (var content in result)
            {
                if (content.Role.HasValue)
                {
                    System.Console.Write("Assistant > ");
                    chatMessageContent = new(
                        content.Role ?? AuthorRole.Assistant,
                        content.ModelId!,
                        content.Content!,
                        content.InnerContent,
                        content.Encoding,
                        content.Metadata
                    );
                }
                System.Console.Write(content.Content);
                chatMessageContent!.Content += content.Content;
            }
            System.Console.WriteLine();
            chatHistory.Add(chatMessageContent!);
        }
    }
}
