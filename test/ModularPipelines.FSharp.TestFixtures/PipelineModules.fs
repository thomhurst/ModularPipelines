namespace ModularPipelines.FSharp.TestFixtures

open System.Threading
open System.Threading.Tasks
open ModularPipelines
open ModularPipelines.Attributes
open ModularPipelines.Context
open ModularPipelines.Extensions
open ModularPipelines.Modules

type DependencyModule() =
    inherit Module<string>()

    override _.ExecuteAsync(
        _context: IModuleContext,
        _cancellationToken: CancellationToken
    ) : Task<string> =
        Task.FromResult("dependency")

[<DependsOn(typeof<DependencyModule>)>]
type DependentModule() =
    inherit Module<string>()

    override _.ExecuteAsync(
        context: IModuleContext,
        _cancellationToken: CancellationToken
    ) : Task<string> =
        task {
            let! dependency = context.GetModule<DependencyModule>()
            return $"{dependency.ValueOrDefault}-dependent"
        }

type DynamicDependentModule() =
    inherit Module<string>()

    override _.DeclareDependencies(dependencies) =
        dependencies.DependsOn<DependencyModule>() |> ignore

    override _.ExecuteAsync(
        context: IModuleContext,
        _cancellationToken: CancellationToken
    ) : Task<string> =
        task {
            let! dependency = context.GetModule<DependencyModule>()
            return $"{dependency.ValueOrDefault}-dynamic"
        }

type PipelineRunner =
    static member RunAsync() =
        Pipeline
            .CreateBuilder()
            .AddModule<DependentModule>()
            .ExecutePipelineAsync()

    static member RunDynamicAsync() =
        Pipeline
            .CreateBuilder()
            .AddModule<DependencyModule>()
            .AddModule<DynamicDependentModule>()
            .ExecutePipelineAsync()
