---
title: Single File C# Example
---

Starting with dotnet 10 (preview 4), you can use the new file-based C# application feature to write scripts in C# without needing to create a full project structure. This is particularly useful for quick scripts or small utilities.

## Using File-Based C# Application with ModularPipelines

To use ModularPipelines in a single file C# application, you can follow these steps:

1.  **Create a C# file**: Create a new file named `example.cs` (or any name with `.cs` extension).
2.  **Add ModularPipelines to your project**: You can add TUnit as a package reference in your file. At the top of your `example.cs`, add the following line:

    ```csharp
    #:package ModularPipelines@2.*
    ```

    Alternatively, you can specify a specific version:

    ```csharp
    #:package ModularPipelines@2.44.45
    ```

3.  **Write your C# code**: Below the package reference, you can write your C# code using ModularPipelines. Here’s a simple example that uses ModularPipelines to check the dotnet version:

    ```csharp
        #!/usr/bin/dotnet run
        #:package ModularPipelines.DotNet@2.44.*
        using ModularPipelines.Attributes;
        using ModularPipelines.Context;
        using ModularPipelines.DotNet.Extensions;
        using ModularPipelines.Host;
        using ModularPipelines.Models;
        using ModularPipelines.Modules;

        await PipelineHostBuilder.Create()
            .AddModule<UpdateDotnetWorkloads>()
            .AddModule<CheckDotnetSdkModule>()
            .ExecutePipelineAsync();

        public class UpdateDotnetWorkloads : Module<CommandResult>
        {
            protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
            {
                return await context.DotNet().DotNetWorkload.Update(token: cancellationToken);
            }
        }

        [DependsOn<UpdateDotnetWorkloads>]
        public class CheckDotnetSdkModule : Module<CommandResult>
        {

            protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
            {
                return await context.DotNet().Sdk.Check(token: cancellationToken);
            }
        }
    ```

4.  **Run your script**: You can run your script directly using the `dotnet run` command. Make sure you have the .NET SDK installed and available in your PATH.

    ```powershell
    dotnet run example.cs
    ```

If you need to convert the file based application to a regular C# project, you can run the following command:

    ```powershell
    dotnet project convert example.cs
    ```
