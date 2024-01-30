using System.ComponentModel;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Planners;

internal class MixologistPlanner
{
    [KernelFunction]
    [Description("Returns back the required steps necessary to create a new cocktail.")]
    [return: Description("The list of steps needed to create a new cocktail")]
    public async Task<string> GenerateRequiredStepsAsync(
        Kernel kernel,
        [Description("Alcoholic or non-Alcoholic")] bool alcoholic,
        [Description("A kind of cocktail drink")] string drink,
        [Description("A type of cuisine that pairs well with the cocktail drink")] string cuisine,
        //[Description("A main dish to eat goes together with dish")] string main_dish,
        [Description("Include the following ingredients in recipe")] string ingredient,
        [Description("An inspiration for the cocktail recipe")] string inspiration
    )
    {
        var additional_instructions = (!alcoholic) ? "Do not include any alcohol. No whiskey, cognac, spirits, VSOP, wine, bourbon, gin, scotch, beer in the ingredients" : "";
        // Prompt the LLM to generate a list of steps to complete the task
        var result = await kernel.InvokePromptAsync($"""
        I want someone who can suggest out of the world and imaginative drink recipes. You are my master mixologist. You will come up with olfactory pleasant {drink} that is appealing and pairs well with the {cuisine} cuisine. Use {ingredient} in your recipe. Avoid eggs or yolk as ingredients. Draw inspiration from an existing cocktail recipe of {inspiration}. Apply understanding of flavor compounds and food pairing theories. Give the drink a unique name. Ingredients must start in a new line. Add a catch phrase for the drink within double quotes. Always provide a rationale. Also try to provide a scientific explanation for why the ingredients were chosen. {additional_instructions}. Provide evidence and citations for where you took the recipe from.
        Cocktail Name: 
        Ingredients:
        Instructions:
        Citations:
        Rationale:###
        """, new() {
            { "drink", drink },
            { "cuisine", cuisine },
            { "ingredient", ingredient },
            { "inspiration", inspiration },
            { "additional_instructions", additional_instructions}
        });

        // Return the plan back to the agent
        return result.ToString();
    }
}

//'additional_instructions':'Do not include any alcohol. No whiskey, cognac, spirits, VSOP, wine, bourbon, gin, scotch, beer in the ingredients'

// var result = await kernel.InvokePromptAsync($"""
//         I want someone who can suggest out of the world and imaginative drink recipes. You are my master mixologist. You will come up with olfactory pleasant {drink} that is appealing and pairs well with the {cuisine} cuisine. Ensure the drink pairs well with {main_dish}. Use {ingredient} in your recipe. Avoid eggs or yolk as ingredients. Draw inspiration from an existing cocktail recipe of {inspiration}. Apply understanding of flavor compounds and food pairing theories. Give the drink a unique name. Ingredients must start in a new line. Add a catch phrase for the drink within double quotes. Always provide a rationale. Also try to provide a scientific explanation for why the ingredients were chosen. {additional_instructions}. Provide evidence and citations for where you took the recipe from.
//         Cocktail Name: 
//         Ingredients:
//         Instructions:
//         Citations:
//         Rationale:###
//         """, new() {
//             { "drink", drink },
//             { "cuisine", cuisine },
//             { "main_dish", main_dish },
//             { "ingredient", ingredient },
//             { "inspiration", inspiration },
//             { "additional_instructions", additional_instructions}
//         });
