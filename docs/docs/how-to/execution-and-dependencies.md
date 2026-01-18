---
title: Execution and Dependencies
sidebar_position: 3
---

# Execution and Dependencies

The default behaviour is for modules to run in parallel, to speed up a pipeline as much as possible.

If you don't want a particular module to start until another one has finished, then you simply add a `[DependsOn<TModule>]` attribute to your module class.

These can chain together as appropriate. And it'll detect if two modules depend on each other.

```csharp
[DependsOn<Module1>] // or [DependsOn(typeof(Module1))] for older language versions
public class Module2 : Module
{
    ...
}
```

## Required vs Optional Dependencies

By default, dependencies declared with `[DependsOn<T>]` are **required**. This means:

1. **Auto-registration**: If the dependency module is not explicitly registered, ModularPipelines will automatically register it for you
2. **Validation**: The pipeline validates that all required dependencies can be resolved before execution

```csharp
// Required dependency (default)
// Module1 will be auto-registered if not explicitly added
[DependsOn<Module1>]
public class Module2 : Module<string>
{
    protected override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Safe to call - Module1 is guaranteed to be registered
        var result = await context.GetModule<Module1>();
        return result.Value;
    }
}
```

### Auto-Registration

When you declare a required dependency, you don't need to explicitly register it:

```csharp
await PipelineHostBuilder.Create()
    .AddModule<Module2>()  // Module1 is auto-registered because Module2 depends on it
    .ExecutePipelineAsync();
```

This simplifies pipeline configuration and ensures all required dependencies are always present. Auto-registration also handles transitive dependencies - if Module1 depends on Module0, both will be auto-registered.

## Optional Dependencies

Use `Optional = true` when a dependency may or may not be present:

```csharp
// Optional dependency - won't be auto-registered
[DependsOn<Module1>(Optional = true)]
public class Module2 : Module<string>
{
    protected override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
- You're using category filters and some dependencies may be excluded
- You want conditional behavior based on what modules are registered

### Category Filters and Optional Dependencies

When using `RunOnlyCategories` to filter which modules run, dependencies in other categories may not execute. Mark such dependencies as optional:

```csharp
[ModuleCategory("test")]
[DependsOn<CompileModule>(Optional = true)]  // CompileModule is in "compile" category
public class TestModule : Module<string>
{
    protected override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var compile = context.GetModuleIfRegistered<CompileModule>();

        if (compile == null)
        {
            // CompileModule was filtered out - handle gracefully
            return "Running tests without compile";
        }

        var result = await compile;
        return result.IsSkipped
            ? "Compile was skipped"
            : $"Compile result: {result.Value}";
    }
}
```

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

## Programmatic Dependencies

You can also declare dependencies programmatically by overriding `DeclareDependencies`:

```csharp
public class Module2 : Module<string>
{
    protected override void DeclareDependencies(IDependencyDeclaration deps)
    {
        deps.DependsOn<Module1>();                    // Required
        deps.DependsOnOptional<Module3>();            // Optional
        deps.DependsOnIf<Module4>(someCondition);     // Conditional
    }
}
```
