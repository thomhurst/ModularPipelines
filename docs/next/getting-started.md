# Getting Started

ModularPipelines pipelines are .NET console applications. Install the .NET 10 SDK first. The quickest path is the project template; you can also add the packages to an existing project manually.

## Create a pipeline from the template[​](#create-a-pipeline-from-the-template "Direct link to Create a pipeline from the template")

Install the templates package:

```
dotnet new install ModularPipelines.Templates
```

Generate a pipeline project. Replace the solution and publish-project paths with paths from your repository:

```
dotnet new modularpipeline -n MyPipeline `

  --solution ../MySolution.slnx `

  --publish-project ../src/MyApp/MyApp.csproj

cd MyPipeline
```

The generated project contains restore, build, test, and publish modules with explicit dependencies. It also includes `appsettings.json`, where you can change the paths and build configuration later.

Run the pipeline like any other .NET application:

```
dotnet run
```

While it runs, the console shows module progress. The final table reports each module's status and duration. If a module fails, its error and command output identify the failed step.

## Add a pipeline manually[​](#add-a-pipeline-manually "Direct link to Add a pipeline manually")

Start with a console project and install the core framework plus the .NET CLI integration:

```
dotnet new console -n MyPipeline

cd MyPipeline

dotnet add package ModularPipelines

dotnet add package ModularPipelines.DotNet
```

Replace `Program.cs` with this complete example, updating the solution path for your repository:

```
using ModularPipelines;

using ModularPipelines.Context;

using ModularPipelines.DotNet.Options;

using ModularPipelines.Extensions;

using ModularPipelines.Models;

using ModularPipelines.Modules;



var builder = Pipeline.CreateBuilder(args);

builder.AddModule<BuildModule>();



await builder.RunAsync();



public sealed class BuildModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.DotNet.BuildAsync(

            new DotNetBuildOptions

            {

                ProjectSolution = "../MySolution.slnx",

                Configuration = "Release",

            },

            cancellationToken: cancellationToken);

    }

}
```

Run it with `dotnet run`. From here, learn how to add dependencies, conditions, and typed results in [Fundamentals](/ModularPipelines/docs/next/fundamentals.md).
