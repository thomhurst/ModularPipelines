---
title: Using F#
sidebar_position: 10
---

# Using F#

ModularPipelines modules can be authored and executed from an F# project. Inherit
from `Module<'T>`, override `ExecuteAsync`, and open
`ModularPipelines.Extensions` for pipeline-builder extension methods.

F# pipelines currently require a JIT-compiled application. Trimming and Native AOT
are not supported because the metadata generators run only for C# compilations. The
package emits `MPAOT001` if an F# project enables `PublishTrimmed` or `PublishAot`.

```fsharp
open System.Threading
open System.Threading.Tasks
open ModularPipelines
open ModularPipelines.Attributes
open ModularPipelines.Configuration
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

use builder = Pipeline.CreateBuilder()

builder
    .AddModule<TestModule>()
    .RunAsync()
    .GetAwaiter()
    .GetResult()
|> ignore
```

F# does not support applying generic attribute types, so declare static module
dependencies with `[<DependsOn(typeof<DependencyModule>)>]`. This overload is a
supported API and has the same auto-registration, validation, optional-dependency,
and cascade-skip behavior as C#'s `[DependsOn<DependencyModule>]`.

For runtime-selected dependencies, override `Configure` and use the fluent
configuration builder:

```fsharp
override _.Configure() =
    ModuleConfiguration.Create()
        .DependsOn<BuildModule>()
        .Build()
```
