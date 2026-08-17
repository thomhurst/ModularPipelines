# Using F\#

ModularPipelines modules can be authored and executed from an F# project. Inherit from `Module<'T>`, override `ExecuteAsync`, and open `ModularPipelines.Extensions` for pipeline-builder extension methods.

```
open System.Threading

open System.Threading.Tasks

open ModularPipelines

open ModularPipelines.Attributes

open ModularPipelines.Context

open ModularPipelines.Extensions

open ModularPipelines.Modules



type BuildModule() =

    inherit Module<string>()



    override _.ExecuteAsync(

        _context: IModuleContext,

        _cancellationToken: CancellationToken

    ) : Task<string> =

        Task.FromResult("built")



[<DependsOn(typeof<BuildModule>)>]

type TestModule() =

    inherit Module<string>()



    override _.ExecuteAsync(

        context: IModuleContext,

        _cancellationToken: CancellationToken

    ) : Task<string> =

        task {

            let! build = context.GetModule<BuildModule>()

            return $"tested {build.ValueOrDefault}"

        }



let builder = Pipeline.CreateBuilder()



builder

    .AddModule<TestModule>()

    .ExecutePipelineAsync()

    .GetAwaiter()

    .GetResult()

|> ignore
```

F# does not support applying generic attribute types, so declare static module dependencies with `[<DependsOn(typeof<DependencyModule>)>]`. This overload is a supported API and has the same auto-registration, validation, optional-dependency, and cascade-skip behavior as C#'s `[DependsOn<DependencyModule>]`.

For dynamic dependencies, override `DeclareDependencies` and use its generic methods:

```
override _.DeclareDependencies(dependencies) =

    dependencies.DependsOn<BuildModule>() |> ignore
```
