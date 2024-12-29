using CustomCopilot.Plugins.FlightTrackerPlugin;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using static System.Environment;

namespace CustomCopilot
{
    internal class Program
    {

        static async Task Main(string[] args)

        {
            Environment.SetEnvironmentVariable("AOI_ENDPOINT_SWDN", "https://azureopenaizhenyu.openai.azure.com");
            Environment.SetEnvironmentVariable("AOI_KEY_SWDN", "c38cc27ce9914aaeb84cb71bc88116d9");

      
            var builder = Kernel.CreateBuilder();
            builder.AddAzureOpenAIChatCompletion("gpt-4o",
               GetEnvironmentVariable("AOI_ENDPOINT_SWDN")!,
                GetEnvironmentVariable("AOI_KEY_SWDN")!);

            builder.Plugins.AddFromObject(new FlightTrackerPlugin("cf62b704fb573a7dbe90976ffefb9a46"), nameof(FlightTrackerPlugin));
        

            var kernel = builder.Build();

            ChatHistory history = [];
            history.AddSystemMessage(@"You're a virtual assistant that helps people track flight and find information.");

        
            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

       
            while (true)
            {
              

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("User > ");
                history.AddUserMessage(Console.ReadLine()!);

                OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
                {
                    MaxTokens = 200
                };

                

                var response = chatCompletionService.GetStreamingChatMessageContentsAsync(
                               history,
                               executionSettings: openAIPromptExecutionSettings,
                               kernel: kernel);


                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\nAssistant > ");

                string combinedResponse = string.Empty;
                await foreach (var message in response)
                {
                   

                    Console.Write(message);
                    combinedResponse += message;
                }

                Console.WriteLine();

            

                history.AddAssistantMessage(combinedResponse);
            }
        }
    }
}