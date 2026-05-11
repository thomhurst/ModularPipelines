---
title: F# Interactive Example
---

F# Interactive (FSI) is a powerful tool for executing F# code snippets and scripts in an interactive environment. It allows you to quickly test and iterate on your F# code without needing to create a full project structure.

## Using F# Interactive with ModularPipelines

1. **Create a new F# script file:** Create a new file named `example.fsx` (or any name with `.fsx` extension).
2. **Add ModularPipelines to your script:** You can add ModularPipelines as a package reference in your F# script. At the top of your `example.fsx`, add the following line:

    ```fsharp
    #r "nuget: ModularPipelines, 3.*"
    ```

    Alternatively, you can specify a specific version:

    ```fsharp
    #r "nuget: ModularPipelines, 3.2.8"
    ```
3. **Write your F# code:** Below the package reference, you can write your F# code using ModularPipelines. Here’s a simple example that uses ModularPipelines to check the dotnet version:

    ```fsharp
    #r "nuget: ModularPipelines.DotNet, 3.*"
    open ModularPipelines.DotNet
    open ModularPipelines.Attributes
    open ModularPipelines.Context
    open ModularPipelines.DotNet.Extensions
    open ModularPipelines.Extensions
    open ModularPipelines.Models
    open ModularPipelines.Modules
    open ModularPipelines
    open System.Threading

    type UpdateDotnetWorkloads() =
        inherit Module<CommandResult>()
        override this.ExecuteAsync (context: IModuleContext, cancellationToken: CancellationToken): Tasks.Task<CommandResult> = 
                context.DotNet().Workload.Update(cancellationToken = cancellationToken)

    /// Generic attributes are not supported in fsharp, so have to use the old way of declaring dependencies
    [<DependsOn(typeof<UpdateDotnetWorkloads>)>]
    type CheckDotnetSdkModule () =
        inherit Module<CommandResult>()
        override this.ExecuteAsync (context: IModuleContext, cancellationToken: CancellationToken): Tasks.Task<CommandResult> = 
                context.DotNet().Sdk.Check(cancellationToken = cancellationToken);

    let args = System.Environment.GetCommandLineArgs()
    let builder = Pipeline.CreateBuilder(args)

    builder.Services
        .RegisterDotNetContext()
        .AddModule<UpdateDotnetWorkloads>()
        .AddModule<CheckDotnetSdkModule>()

    builder.Build().RunAsync()
    |> Async.AwaitTask
    |> Async.RunSynchronously   
    ``` 
4. **Run your F# script:** You can run your F# script using the F# Interactive environment. If you are using Visual Studio, you can simply open the `example.fsx` file and execute it. Alternatively, you can run it from the command line using:

    ```powershell
    dotnet fsi example.fsx
    ```       