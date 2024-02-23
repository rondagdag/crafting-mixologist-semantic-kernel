
# Crafting an AI Mixologist Using Semantic Kernel

Prompts play a crucial role in communicating and directing the behavior of Large Language Models (LLMs) AI. Semantic Kernel is like a mixologist who looks at available ingredients and crafts new cocktail recipes. Semantic Kernel looks at different plugins, prompts, and memory stores to create an execution plan. Attend the presentation to learn about Semantic Kernel and build an AI Mixologist creative enough to suggest new cocktails. Cheers!

## Presentation

[Slides](crafting-mixologist-semantic-kernel.pdf)

## Code

This folder contains a few C# Jupyter Notebooks to demonstrate and get started with
the Semantic Kernel. 

To run the notebooks, we recommend the following steps:

- [Install .NET 8](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Install Visual Studio Code (VS Code)](https://code.visualstudio.com)
- Launch VS Code and [install the "Polyglot" extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-interactive-vscode).
  Min version required: v1.0.4606021 (Dec 2023).

## Set your OpenAI API key

To start using these notebooks, be sure to add the appropriate API keys to `config/settings.json`.

You can create the file manually or run [the Setup notebook](0-AI-settings.ipynb).

For Azure OpenAI:

```json
{
  "type": "azure",
  "model": "...", // Azure OpenAI Deployment Name
  "endpoint": "...", // Azure OpenAI endpoint
  "apikey": "..." // Azure OpenAI key
}
```

For OpenAI:

```json
{
  "type": "openai",
  "model": "gpt-3.5-turbo", // OpenAI model name
  "apikey": "...", // OpenAI API Key
  "org": "" // only for OpenAI accounts with multiple orgs
}
```

If you need an Azure OpenAI key, go [here](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/quickstart?pivots=rest-api).
If you need an OpenAI key, go [here](https://platform.openai.com/account/api-keys)

# Topics

Before starting, make sure you configured `config/settings.json`,
see the previous section.

For a quick dive, look at the [getting started notebook](notebooks/00-getting-started.ipynb).

1. [Loading and configuring Semantic Kernel, Semantic Functions at runtime (i.e. inline functions)](notebooks/01-semantic-function-inline.ipynb)
2. [Running AI prompts from file](notebooks/02-semantic-function-type.ipynb)
3. [Using Kernel Arguments to Build a Chat Experience](notebooks/03-kernel-chat.ipynb)
4. [Creating images with DALL-E 3](notebooks/04-DALL-E-3.ipynb)
5. [Use GPT Vision Model](notebooks/05-GPTVision.ipynb)
6. [Introduction to the Planner](notebooks/06-using-the-planner.ipynb)
7. [Building Semantic Memory and Save the Embeddings](notebooks/07-memory-and-save-embeddings.ipynb)
8. [Load and Search Embeddings](notebooks/08-memory-and-embeddings.ipynb)
9. [First Copilot](notebooks/09-first-copilot.ipynb)
10. [Multi Agents](notebooks/10-multi-agents.ipynb)

# Run notebooks in the browser with JupyterLab

You can run the notebooks also in the browser with JupyterLab. These steps
should be sufficient to start:

Install Python 3, Pip and .NET 7 in your system, then:

    pip install jupyterlab
    dotnet tool install -g Microsoft.dotnet-interactive
    dotnet tool update -g Microsoft.dotnet-interactive
    dotnet interactive jupyter install

This command will confirm that Jupyter now supports C# notebooks:

    jupyter kernelspec list

Enter the notebooks folder, and run this to launch the browser interface:

    jupyter-lab

![image](https://user-images.githubusercontent.com/371009/216756924-41657aa0-5574-4bc9-9bdb-ead3db7bf93a.png)

# Troubleshooting

## Nuget

If you are unable to get the Nuget package, first list your Nuget sources:

```sh
dotnet nuget list source
```

If you see `No sources found.`, add the NuGet official package source:

```sh
dotnet nuget add source "https://api.nuget.org/v3/index.json" --name "nuget.org"
```

Run `dotnet nuget list source` again to verify the source was added.

## Polyglot Notebooks

If somehow the notebooks don't work, run these commands:

- Install .NET Interactive: `dotnet tool install -g Microsoft.dotnet-interactive`
- Register .NET kernels into Jupyter: `dotnet interactive jupyter install` (this might return some errors, ignore them)
- If you are still stuck, read the following pages:
  - https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-interactive-vscode
  - https://devblogs.microsoft.com/dotnet/net-core-with-juypter-notebooks-is-here-preview-1/
  - https://docs.servicestack.net/jupyter-notebooks-csharp
  - https://developers.refinitiv.com/en/article-catalog/article/using--net-core-in-jupyter-notebook

Note: ["Polyglot Notebooks" used to be called ".NET Interactive Notebooks"](https://devblogs.microsoft.com/dotnet/dotnet-interactive-notebooks-is-now-polyglot-notebooks/),
so you might find online some documentation referencing the old name.
