# Single File C# Example

Starting with .NET 10, file-based apps let you write and run C# without creating a project file. They are useful for quick scripts and small utilities.

## Using File-Based C# Application with ModularPipelines[​](#using-file-based-c-application-with-modularpipelines "Direct link to Using File-Based C# Application with ModularPipelines")

Install the .NET 10 SDK, then follow these steps:

1. **Create a C# file**: Create `example.cs` (or another file with a `.cs` extension).

2. **Reference ModularPipelines**: Add the v4 DotNet integration package at the top of the file. It brings in the core `ModularPipelines` package transitively.

   ```
   #:package ModularPipelines.DotNet@4.*
   ```

3. **Write the pipeline**: This example updates installed .NET workloads, then checks the SDK:

   ```
   #!/usr/bin/env -S dotnet --

   #:package ModularPipelines.DotNet@4.*



   using ModularPipelines;

   using ModularPipelines.Attributes;

   using ModularPipelines.Context;

   using ModularPipelines.Extensions;

   using ModularPipelines.Models;

   using ModularPipelines.Modules;



   var builder = Pipeline.CreateBuilder(args);



   builder

       .AddModule<UpdateDotnetWorkloads>()

       .AddModule<CheckDotnetSdkModule>();



   await builder.ExecutePipelineAsync();



   public class UpdateDotnetWorkloads : Module<CommandResult>

   {

       protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)

       {

           return await context.Tools.DotNet.Workload.UpdateAsync(cancellationToken: cancellationToken);

       }

   }



   [DependsOn<UpdateDotnetWorkloads>]

   public class CheckDotnetSdkModule : Module<CommandResult>

   {

       protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)

       {

           return await context.Tools.DotNet.Sdk.CheckAsync(cancellationToken: cancellationToken);

       }

   }
   ```

4. **Run the file-based app**:

   ```
   dotnet run --file example.cs
   ```

To convert the file-based app to a regular C# project, run:

```
dotnet project convert example.cs
```
