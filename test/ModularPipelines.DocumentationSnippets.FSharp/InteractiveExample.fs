module ModularPipelines.DocumentationSnippets.FSharp.InteractiveExample

open ModularPipelines.DotNet.Extensions
open ModularPipelines.DotNet.Services
open ModularPipelines.Extensions
open ModularPipelines
open System.Threading

type UpdateDotnetWorkloads() =
    inherit Module<CommandResult>()
    override this.ExecuteAsync (context: IModuleContext, cancellationToken: CancellationToken): System.Threading.Tasks.Task<CommandResult> =
            context.Tools.Get<IDotNet>().Workload.UpdateAsync(cancellationToken = cancellationToken)

/// Generic attributes are not supported in fsharp, so have to use the old way of declaring dependencies
[<DependsOn(typeof<UpdateDotnetWorkloads>)>]
type CheckDotnetSdkModule () =
    inherit Module<CommandResult>()
    override this.ExecuteAsync (context: IModuleContext, cancellationToken: CancellationToken): System.Threading.Tasks.Task<CommandResult> =
            context.Tools.Get<IDotNet>().Sdk.CheckAsync(cancellationToken = cancellationToken)

let args = System.Environment.GetCommandLineArgs()
let builder = Pipeline.CreateBuilder(args)
builder.Services.RegisterDotNetContext() |> ignore

builder
    .AddModule<UpdateDotnetWorkloads>()
    .AddModule<CheckDotnetSdkModule>()
|> ignore

builder.RunAsync()
|> Async.AwaitTask
|> Async.RunSynchronously
|> ignore
