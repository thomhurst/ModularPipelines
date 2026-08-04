---
title: Execution and Dependencies
sidebar_position: 3
---

# Execution and Dependencies

The default behaviour is for modules to run in parallel, to speed up a pipeline as much as possible.

If you don't want a particular module to start until another one has finished, then you simply add a `[DependsOn<TModule>]` attribute to your module class.

These can chain together as appropriate. And it'll detect if two modules depend on each other.

```csharp
[DependsOn<Module1>] // F#: [<DependsOn(typeof<Module1>)>]
public class Module2 : Module
{
    ...
}
```

## Required vs Optional Dependencies

By default, dependencies declared with `[DependsOn<T>]` are **required**. This means:

1. **Auto-registration**: If the dependency module is not explicitly registered, ModularPipelines will automatically register it for you
2. **Validation**: The pipeline validates that all required dependencies can be resolved before execution
3. **Cascade skipping**: If a required dependency is skipped by a run condition or category filter, the dependent module is skipped too

```csharp
// Required dependency (default)
// Module1 will be auto-registered if not explicitly added
[DependsOn<Module1>]
public class Module2 : Module<string>
{
    protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Safe to call - Module1 is guaranteed to be registered
        var result = await context.GetModule<Module1>();
        return result.Value;
    }
}
```

## Exporting the Dependency Graph

Export the resolved graph without executing modules from the command line:

```bash
dotnet run -- --graph mermaid dependency-graph.mmd
dotnet run -- --graph dot dependency-graph.dot
dotnet run -- --graph json dependency-graph.json
```

The path is optional. The defaults are `dependency-graph.mmd`, `dependency-graph.dot`,
and `dependency-graph.json`. Graph nodes include the module category, estimated duration,
and skip status. Conditions that require runtime results or asynchronous work are shown as
unresolved. Edges point from each dependency to the module that depends on it.

Paths containing a directory separator can include `=` directly. For an ambiguous filename
containing `=`, use the explicit path option so it is not treated as host configuration:

```bash
dotnet run -- --graph json --graph-path branch=main.json
```

You can also export programmatically:

```csharp
using ModularPipelines.Enums;

using var builder = Pipeline.CreateBuilder(args);
builder.AddModule<Module2>();

await builder.ExportDependencyGraphAsync(
    DependencyGraphFormat.Mermaid,
    "dependency-graph.mmd");
```

When the `ModularPipelines.GitHub` integration writes `GITHUB_STEP_SUMMARY`, it includes
the Mermaid dependency flowchart alongside the run Gantt and result table.

### Auto-Registration

When you declare a required dependency, you don't need to explicitly register it:

```csharp
var builder = Pipeline.CreateBuilder(args);
builder.AddModule<Module2>(); // Module1 is auto-registered because Module2 depends on it

await builder.ExecutePipelineAsync();
```

This simplifies pipeline configuration and ensures all required dependencies are always present. Auto-registration also handles transitive dependencies - if Module1 depends on Module0, both will be auto-registered.

## Optional Dependencies

Use `Optional = true` when a dependency may or may not be present:

```csharp
// Optional dependency - won't be auto-registered
[DependsOn<Module1>(Optional = true)]
public class Module2 : Module<string>
{
    protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Use GetModuleIfRegistered for optional dependencies
        var module1 = context.GetModuleIfRegistered<Module1>();

        if (module1 != null)
        {
            var result = await module1;
            return $"Got result: {result.Value}";
        }

        return "Module1 not available";
    }
}
```

Optional dependencies are useful when:

- A module can work with or without another module's output
- A module should still run when a dependency is excluded by a category filter or run condition
- You want conditional behavior based on what modules are registered

### Category Filters and Skipped Dependencies

When a category filter skips a required dependency, ModularPipelines cascade-skips the dependent module and any modules that require it. Use an optional dependency when the dependent module should still execute:

```csharp
[ModuleCategory("test")]
[DependsOn<CompileModule>(Optional = true)]  // CompileModule is in "compile" category
public class TestModule : Module<string>
{
    protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var compile = context.GetModuleIfRegistered<CompileModule>();

        if (compile == null)
        {
            // Optional dependencies are not auto-registered
            return "Running tests without compile";
        }

        var result = await compile;
        return result.SkipDecisionOrDefault is not null
            ? "Compile was skipped"
            : $"Compile result: {result.Value}";
    }
}
```

If the dependency above were required (`[DependsOn<CompileModule>]`), `TestModule` would be skipped whenever the category filter skipped `CompileModule`.

## Accessing Dependency Results

Use `GetModule<T>()` for required dependencies - it throws if the module is not registered:

```csharp
var result = await context.GetModule<Module1>();
```

Use `GetModuleIfRegistered<T>()` for optional dependencies - it returns null if not registered:

```csharp
var module = context.GetModuleIfRegistered<Module1>();
if (module != null)
{
    var result = await module;
    // Use the result
}
```

## Fluent Dependencies

Declare runtime-selected dependencies through the module's `Configure()` method:

```csharp
public class Module2 : Module<string>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .DependsOn<Module1>()                    // Required
        .DependsOnOptional<Module3>()            // Optional
        .DependsOnIf<Module4>(someCondition)     // Required when true
        .Build();
}
```
