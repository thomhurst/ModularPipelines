# ModularPipelines v2 to v3 Migration Guide

## Overview

ModularPipelines v3.0 represents a major architectural shift from inheritance-based modules to composition-based modules. This guide will help you migrate your v2.x pipelines to v3.0.

## Table of Contents

- [Major Changes](#major-changes)
- [Breaking Changes](#breaking-changes)
- [Migration Steps](#migration-steps)
- [Module Base Class Changes](#module-base-class-changes)
- [SubModule API Changes](#submodule-api-changes)
- [Common Migration Scenarios](#common-migration-scenarios)
- [Troubleshooting](#troubleshooting)

## Major Changes

### Architecture: Inheritance to Composition

**v2.x**: Modules inherited from `ModuleBase<T>` which provided numerous built-in methods and properties.

**v3.0**: Modules inherit from minimal `Module<T>` base class. Functionality is accessed through the `IPipelineContext` parameter.

### Benefits of Composition-Based Architecture

1. **Cleaner Dependencies**: Explicit dependencies through method parameters
2. **Better Testability**: No hidden state in base classes
3. **Simpler Base Class**: Minimal `Module<T>` with only essential functionality
4. **Explicit Context**: All pipeline services accessed via `IPipelineContext`

## Breaking Changes

### 1. Module Base Class

| Aspect | v2.x | v3.0 |
|--------|------|------|
| Base Class | `ModuleBase<T>` | `Module<T>` |
| Context Access | Inherited properties | `IPipelineContext` parameter |
| Execution Method | `ExecuteAsync(context, cancellation)` | `ExecuteAsync(context, cancellation)` |

### 2. SubModule API

| Aspect | v2.x | v3.0 |
|--------|------|------|
| Signature | `SubModule(name, action)` | `SubModule(context, name, action, cancellationToken)` |
| Context | Implicit (from base) | Explicit parameter |
| Cancellation | Limited | Full support |

### 3. Removed Classes

The following classes have been removed in v3.0:

- `ModuleBase<T>` → Use `Module<T>`
- `SubModuleFailedException` → Exceptions propagate directly
- Various base class helper methods → Use `IPipelineContext` extension methods

## Migration Steps

### Step 1: Update Module Base Class

**Before (v2.x)**:
```csharp
using ModularPipelines.Modules;

public class MyModule : ModuleBase<string>
{
    protected override async Task<string?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        // Your code
    }
}
```

**After (v3.0)**:
```csharp
using ModularPipelines.Modules;

public class MyModule : Module<string>
{
    public override async Task<string?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        // Your code
    }
}
```

**Changes**:
- `ModuleBase<T>` → `Module<T>`
- `protected override` → `public override` (if you prefer, though `public` is now required)
- No other changes to method signature needed

### Step 2: Update SubModule Calls

**Before (v2.x)**:
```csharp
public class ProcessFilesModule : ModuleBase<string[]>
{
    protected override async Task<string[]?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        var files = new[] { "file1.txt", "file2.txt", "file3.txt" };

        return await files.ToAsyncProcessorBuilder()
            .SelectAsync(file => SubModule(file, async () =>
            {
                return await ProcessFile(file);
            }))
            .ProcessInParallel();
    }
}
```

**After (v3.0)**:
```csharp
public class ProcessFilesModule : Module<string[]>
{
    public override async Task<string[]?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        var files = new[] { "file1.txt", "file2.txt", "file3.txt" };

        return await files.ToAsyncProcessorBuilder()
            .SelectAsync(file => SubModule(context, file, async () =>
            {
                return await ProcessFile(file);
            }, cancellationToken))
            .ProcessInParallel();
    }
}
```

**Changes**:
- Add `context` as first parameter to `SubModule()`
- Add `cancellationToken` as last parameter to `SubModule()`
- Change base class from `ModuleBase<T>` to `Module<T>`

### Step 3: Access Pipeline Services via Context

All pipeline services that were previously accessible via base class properties are now accessed through the `IPipelineContext` parameter.

**Before (v2.x)**:
```csharp
// Accessing services via base class
protected override async Task<string?> ExecuteAsync(
    IPipelineContext context,
    CancellationToken cancellationToken)
{
    // These were available directly from base class
    var gitInfo = await Git.Information;
    var result = await DotNet.Test(...);
    Logger.LogInformation("Message");
}
```

**After (v3.0)**:
```csharp
// Accessing services via context
public override async Task<string?> ExecuteAsync(
    IPipelineContext context,
    CancellationToken cancellationToken)
{
    // Access everything through context
    var gitInfo = await context.Git().Information;
    var result = await context.DotNet().Test(...);
    context.Logger.LogInformation("Message");
}
```

## Module Base Class Changes

### Property Access Pattern

| Service | v2.x Access | v3.0 Access |
|---------|-------------|-------------|
| Git | `Git.*` | `context.Git().*` |
| DotNet | `DotNet.*` | `context.DotNet().*` |
| Docker | `Docker.*` | `context.Docker().*` |
| Logger | `Logger.*` | `context.Logger.*` |
| File System | `FileSystem.*` | `context.FileSystem.*` |
| Environment | `Environment.*` | `context.Environment.*` |

### Complete Example

**Before (v2.x)**:
```csharp
public class BuildModule : ModuleBase<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        Logger.LogInformation("Building project...");

        var solutionFile = FileSystem.RootDirectory
            .FindFile(x => x.Extension == ".sln");

        return await DotNet.Build(new DotNetBuildOptions
        {
            ProjectSolutionDirectoryDllExe = solutionFile
        }, cancellationToken);
    }
}
```

**After (v3.0)**:
```csharp
public class BuildModule : Module<CommandResult>
{
    public override async Task<CommandResult?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Building project...");

        var solutionFile = context.FileSystem.RootDirectory
            .FindFile(x => x.Extension == ".sln");

        return await context.DotNet().Build(new DotNetBuildOptions
        {
            ProjectSolutionDirectoryDllExe = solutionFile
        }, cancellationToken);
    }
}
```

## SubModule API Changes

### Overview

The SubModule API has been restored in v3.0 with an improved design that requires explicit context and cancellation token parameters.

### Signature Changes

**v2.x Signatures**:
```csharp
// Async with result
protected Task<T> SubModule<T>(string name, Func<Task<T>> action)

// Sync with result
protected Task<T> SubModule<T>(string name, Func<T> action)

// Async without result
protected Task SubModule(string name, Func<Task> action)

// Sync without result
protected Task SubModule(string name, Action action)
```

**v3.0 Signatures**:
```csharp
// Async with result
protected Task<T> SubModule<T>(
    IPipelineContext context,
    string name,
    Func<Task<T>> action,
    CancellationToken cancellationToken = default)

// Sync with result
protected Task<T> SubModule<T>(
    IPipelineContext context,
    string name,
    Func<T> action,
    CancellationToken cancellationToken = default)

// Async without result
protected Task SubModule(
    IPipelineContext context,
    string name,
    Func<Task> action,
    CancellationToken cancellationToken = default)

// Sync without result
protected Task SubModule(
    IPipelineContext context,
    string name,
    Action action,
    CancellationToken cancellationToken = default)
```

### Migration Examples

#### Example 1: Simple Async SubModule

**v2.x**:
```csharp
await SubModule("Download File", async () =>
{
    await DownloadFileAsync(url);
});
```

**v3.0**:
```csharp
await SubModule(context, "Download File", async () =>
{
    await DownloadFileAsync(url);
}, cancellationToken);
```

#### Example 2: SubModule with Return Value

**v2.x**:
```csharp
var results = await items.ToAsyncProcessorBuilder()
    .SelectAsync(item => SubModule(item.Name, async () =>
    {
        return await ProcessItemAsync(item);
    }))
    .ProcessInParallel();
```

**v3.0**:
```csharp
var results = await items.ToAsyncProcessorBuilder()
    .SelectAsync(item => SubModule(context, item.Name, async () =>
    {
        return await ProcessItemAsync(item);
    }, cancellationToken))
    .ProcessInParallel();
```

#### Example 3: Synchronous SubModule

**v2.x**:
```csharp
await SubModule("Calculate", () =>
{
    var result = PerformCalculation();
    Logger.LogInformation("Result: {Result}", result);
});
```

**v3.0**:
```csharp
await SubModule(context, "Calculate", () =>
{
    var result = PerformCalculation();
    context.Logger.LogInformation("Result: {Result}", result);
}, cancellationToken);
```

## Common Migration Scenarios

### Scenario 1: Simple Module

**v2.x**:
```csharp
public class HelloWorldModule : ModuleBase<string>
{
    protected override Task<string?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        Logger.LogInformation("Hello, World!");
        return Task.FromResult<string?>("Hello, World!");
    }
}
```

**v3.0**:
```csharp
public class HelloWorldModule : Module<string>
{
    public override Task<string?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Hello, World!");
        return Task.FromResult<string?>("Hello, World!");
    }
}
```

### Scenario 2: Module with Dependencies

**v2.x**:
```csharp
[DependsOn<BuildModule>]
public class TestModule : ModuleBase<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        Logger.LogInformation("Running tests...");

        return await DotNet.Test(new DotNetTestOptions
        {
            Configuration = "Release"
        }, cancellationToken);
    }
}
```

**v3.0**:
```csharp
[DependsOn<BuildModule>]
public class TestModule : Module<CommandResult>
{
    public override async Task<CommandResult?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Running tests...");

        return await context.DotNet().Test(new DotNetTestOptions
        {
            Configuration = "Release"
        }, cancellationToken);
    }
}
```

### Scenario 3: Module with SubModules and Loops

**v2.x**:
```csharp
public class PublishModule : ModuleBase<string[]>
{
    protected override async Task<string[]?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        var projects = FileSystem.RootDirectory
            .FindFiles(x => x.Name.EndsWith(".csproj"));

        return await projects.ToAsyncProcessorBuilder()
            .SelectAsync(project => SubModule(project.Name, async () =>
            {
                Logger.LogInformation("Publishing {Project}", project.Name);
                await DotNet.Publish(new DotNetPublishOptions
                {
                    ProjectSolutionDirectoryDllExe = project
                }, cancellationToken);
                return project.Name;
            }))
            .ProcessInParallel();
    }
}
```

**v3.0**:
```csharp
public class PublishModule : Module<string[]>
{
    public override async Task<string[]?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        var projects = context.FileSystem.RootDirectory
            .FindFiles(x => x.Name.EndsWith(".csproj"));

        return await projects.ToAsyncProcessorBuilder()
            .SelectAsync(project => SubModule(context, project.Name, async () =>
            {
                context.Logger.LogInformation("Publishing {Project}", project.Name);
                await context.DotNet().Publish(new DotNetPublishOptions
                {
                    ProjectSolutionDirectoryDllExe = project
                }, cancellationToken);
                return project.Name;
            }, cancellationToken))
            .ProcessInParallel();
    }
}
```

### Scenario 4: Module with Skip Logic

**v2.x**:
```csharp
public class DeployModule : ModuleBase<CommandResult>
{
    protected override SkipDecision ShouldSkip(IPipelineContext context)
    {
        if (context.Environment.EnvironmentVariables
            .TryGet("DEPLOY_ENABLED", out var value)
            && value == "false")
        {
            return SkipDecision.Skip("Deployment disabled");
        }

        return SkipDecision.DoNotSkip;
    }

    protected override async Task<CommandResult?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        // Deploy logic
    }
}
```

**v3.0**:
```csharp
public class DeployModule : Module<CommandResult>, IModuleSkipCondition
{
    public SkipDecision ShouldSkip(IPipelineContext context)
    {
        if (context.Environment.EnvironmentVariables
            .TryGet("DEPLOY_ENABLED", out var value)
            && value == "false")
        {
            return SkipDecision.Skip("Deployment disabled");
        }

        return SkipDecision.DoNotSkip;
    }

    public override async Task<CommandResult?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        // Deploy logic
    }
}
```

**Note**: Skip logic now requires implementing `IModuleSkipCondition` interface.

## Troubleshooting

### Common Issues and Solutions

#### Issue 1: Cannot access Git/DotNet/etc. directly

**Error**:
```
CS0103: The name 'Git' does not exist in the current context
```

**Solution**:
Access via context: `context.Git()`

#### Issue 2: SubModule signature mismatch

**Error**:
```
CS1501: No overload for method 'SubModule' takes 2 arguments
```

**Solution**:
Add `context` as first parameter and `cancellationToken` as last parameter:
```csharp
// Old: SubModule("name", action)
// New: SubModule(context, "name", action, cancellationToken)
```

#### Issue 3: ModuleBase not found

**Error**:
```
CS0246: The type or namespace name 'ModuleBase<>' could not be found
```

**Solution**:
Change `ModuleBase<T>` to `Module<T>`

#### Issue 4: Protected override ExecuteAsync not recognized

**Error**:
```
CS0621: Virtual or abstract members cannot be private
```

**Solution**:
Change from `protected override` to `public override`:
```csharp
public override async Task<string?> ExecuteAsync(...)
```

## Migration Checklist

Use this checklist to ensure you've migrated all aspects:

- [ ] Changed all `ModuleBase<T>` to `Module<T>`
- [ ] Changed `protected override` to `public override` for `ExecuteAsync`
- [ ] Updated all property access from base class to `context.*`
- [ ] Updated all `Git.*` to `context.Git().*`
- [ ] Updated all `DotNet.*` to `context.DotNet().*`
- [ ] Updated all `Logger.*` to `context.Logger.*`
- [ ] Updated all `FileSystem.*` to `context.FileSystem.*`
- [ ] Updated all SubModule calls to include `context` and `cancellationToken`
- [ ] Implemented `IModuleSkipCondition` if using `ShouldSkip`
- [ ] Implemented `IModuleRetryPolicy` if using retry logic
- [ ] Tested all modules compile successfully
- [ ] Tested all modules execute correctly
- [ ] Updated any custom base classes
- [ ] Updated documentation

## Getting Help

If you encounter issues during migration:

1. **Check Documentation**: Review the v3.0 documentation at the ModularPipelines website
2. **Example Modules**: Look at the examples in `src/ModularPipelines.Examples/`
3. **Build Pipeline**: Check `src/ModularPipelines.Build/` for real-world v3.0 modules
4. **GitHub Issues**: Report bugs or ask questions at https://github.com/thomhurst/ModularPipelines/issues

## Summary

The v2 to v3 migration primarily involves:

1. **Base Class**: `ModuleBase<T>` → `Module<T>`
2. **Context Access**: Direct property access → `context.*`
3. **SubModule**: Add `context` and `cancellationToken` parameters
4. **Skip Logic**: Implement `IModuleSkipCondition` interface

The composition-based architecture provides better testability, cleaner dependencies, and a more maintainable codebase. While the migration requires updates to your modules, the changes are straightforward and follow consistent patterns.

---

**Version**: 3.0.0
**Last Updated**: 2025-11-09
**Breaking Changes**: Yes - API changes required
