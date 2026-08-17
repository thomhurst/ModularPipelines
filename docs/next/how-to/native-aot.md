# Trimming and Native AOT

The core `ModularPipelines` package supports trimmed and Native AOT C# applications when the pipeline can be described at compile time. Its C# source generator emits the module, dependency, hook, command-option, and secret metadata needed by the runtime.

F# pipeline projects are not trim or Native AOT certified. The package emits `MPAOT001` when an F# project enables `PublishTrimmed` or `PublishAot`, because the C# source generators cannot emit metadata for an `.fsproj`. Keep F# pipelines JIT-compiled, or move their module declarations and registrations into a C# project.

## Supported pipeline shape[​](#supported-pipeline-shape "Direct link to Supported pipeline shape")

Use generic registration for every module:

```
var builder = Pipeline.CreateBuilder(args);



builder

    .AddModule<BuildModule>()

    .AddModule<TestModule>();



await builder.ExecutePipelineAsync();
```

The source generator must run in the application project. Statically declared `DependsOn<TModule>` dependencies, module lifecycle attributes, command option attributes, and `SecretValue` properties then use generated metadata without runtime reflection.

For JSON, use a source-generated `JsonSerializerContext` and the `JsonTypeInfo` overloads on `IJsonContext`. The convenience overloads that accept only a value or `JsonSerializerOptions` are marked as requiring dynamic code and unreferenced members.

## Unsupported dynamic scenarios[​](#unsupported-dynamic-scenarios "Direct link to Unsupported dynamic scenarios")

Native AOT does not support pipeline shapes introduced only at runtime:

* assembly scanning through `AddModulesFromAssembly` or plugin assembly loading;
* module or result types supplied dynamically after compilation;
* selector dependencies such as `DependsOnAllModulesInheritingFrom<T>`, `DependsOnModulesWithTag`, `DependsOnModulesInCategory`, custom `DependsOnBaseAttribute` implementations, custom `DependsOnAttribute` subclasses, and similar runtime predicates; use built-in explicit `DependsOn<T>` dependencies instead;
* distributed type-erased `ModuleResult` JSON serialization and runtime history repositories;
* reflection-based XML, YAML, or JSON serialization.

These APIs remain available to JIT-compiled applications and carry trim/AOT annotations where appropriate. Tool integration packages are not AOT-certified unless their package explicitly declares `IsAotCompatible`.

## Validate an application[​](#validate-an-application "Direct link to Validate an application")

Enable the analyzers and publish for a concrete runtime identifier:

```
<PropertyGroup>

  <PublishAot>true</PublishAot>

  <PublishTrimmed>true</PublishTrimmed>

  <EnableTrimAnalyzer>true</EnableTrimAnalyzer>

  <EnableAotAnalyzer>true</EnableAotAnalyzer>

</PropertyGroup>
```

```
dotnet publish -c Release -r linux-x64 --self-contained true
```

The repository validates a representative pipeline with modules, dependencies, lifecycle hooks, command execution, generated command metadata, and secret masking in both trimmed and Native AOT CI lanes.
