# ModularPipelines V3 Release Notes

**The biggest release yet.** V3 modernizes the entire API to match ASP.NET Core patterns you already know, adds powerful new features, and makes your pipelines cleaner and more maintainable.

## Highlights

### ASP.NET Core-Style Builder Pattern

No more callbacks. Direct property access, just like ASP.NET Core minimal APIs.

```csharp
// Before (V2)
await PipelineHostBuilder.Create()
    .ConfigureAppConfiguration((context, builder) => { ... })
    .ConfigureServices((context, collection) => { ... })
    .ExecutePipelineAsync();

// After (V3)
var builder = Pipeline.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddModule<BuildModule>();
await builder.Build().RunAsync();
```

If you've used ASP.NET Core, this feels instantly familiar.

### Fluent Module Configuration

Configure module behavior with a clean, fluent API instead of scattered property overrides.

```csharp
// Before (V2) - properties scattered across the class
protected internal override TimeSpan Timeout => TimeSpan.FromMinutes(5);
protected override AsyncRetryPolicy<string?> RetryPolicy => ...;
protected internal override Task<SkipDecision> ShouldSkip(...) => ...;

// After (V3) - everything in one place
protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
    .WithTimeout(TimeSpan.FromMinutes(5))
    .WithRetryCount(3)
    .WithSkipWhen(ctx => ctx.Git().Information.BranchName != "main"
        ? SkipDecision.Skip("Only runs on main")
        : SkipDecision.DoNotSkip)
    .Build();
```

### Type-Safe Result Handling

Module results are now discriminated unions. Pattern matching gives you compile-time safety.

```csharp
var result = await context.GetModule<BuildModule>();

return result switch
{
    ModuleResult<BuildOutput>.Success { Value: var output } => Deploy(output),
    ModuleResult.Skipped => null,
    ModuleResult.Failure { Exception: var ex } => throw ex,
    _ => null
};
```

Or use the simpler helpers for quick migrations:
```csharp
if (result.IsSuccess)
{
    var value = result.ValueOrDefault;
}
```

## New Features

### Dynamic Dependencies

Declare dependencies programmatically based on runtime conditions.

```csharp
protected override void DeclareDependencies(IDependencyDeclaration deps)
{
    deps.DependsOn<RequiredModule>();
    deps.DependsOnOptional<OptionalModule>();
    deps.DependsOnIf<ProductionModule>(Environment.IsProduction);
}
```

### Powerful Dependency Attributes

```csharp
// Depend on all modules in a category
[DependsOnModulesInCategory("Build")]
public class TestModule : Module<TestResults> { }

// Depend on all modules with a tag
[DependsOnModulesWithTag("database")]
public class MigrationModule : Module<bool> { }
```

### Conditional Execution Attributes

```csharp
[RunOnLinux]
public class LinuxModule : Module<string> { }

[RunOnWindowsOnly]  // Skips on other platforms
public class WindowsOnlyModule : Module<string> { }

[SkipIf(typeof(IsNotMainBranchCondition))]
public class MainBranchModule : Module<string> { }

[RunIfAll(typeof(IsCI), typeof(IsMainBranch))]
public class CIMainModule : Module<string> { }
```

### Module Tags and Categories

Organize modules for easier management.

```csharp
[ModuleTag("critical")]
[ModuleTag("deployment")]
[ModuleCategory("Infrastructure")]
public class DeployModule : Module<DeployResult> { }
```

### Pipeline Validation

Catch configuration errors before execution.

```csharp
var validation = await builder.ValidateAsync();
if (validation.HasErrors)
{
    foreach (var error in validation.Errors)
    {
        Console.WriteLine($"[{error.Category}] {error.Message}");
    }
}
```

### Plugin System

Create reusable pipeline extensions.

```csharp
public class MyPlugin : IModularPipelinesPlugin
{
    public string Name => "MyPlugin";

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IMyService, MyService>();
    }

    public void ConfigurePipeline(PipelineBuilder builder)
    {
        builder.Services.AddModule<PluginModule>();
    }
}

[assembly: ModularPipelinesPlugin(typeof(MyPlugin))]
```

### Enhanced Lifecycle Hooks

New overridable methods for fine-grained control.

```csharp
protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken ct) { }
protected override Task OnAfterExecuteAsync(IModuleContext context, ModuleResult<T> result, CancellationToken ct) { }
protected override Task OnSkippedAsync(IModuleContext context, SkipDecision decision, CancellationToken ct) { }
protected override Task OnFailedAsync(IModuleContext context, Exception ex, CancellationToken ct) { }
```

## Breaking Changes

### Entry Point

| V2 | V3 |
|----|-----|
| `PipelineHostBuilder.Create()` | `Pipeline.CreateBuilder(args)` |
| `.ConfigureAppConfiguration(callback)` | `builder.Configuration` |
| `.ConfigureServices(callback)` | `builder.Services` |
| `.ConfigurePipelineOptions(callback)` | `builder.Options` |
| `.AddModule<T>()` on builder | `builder.Services.AddModule<T>()` |
| `.ExecutePipelineAsync()` | `.Build().RunAsync()` |

### Module API

| V2 | V3 |
|----|-----|
| `IPipelineContext` in ExecuteAsync | `IModuleContext` |
| `GetModule<T>()` on module | `context.GetModule<T>()` |
| `Timeout` property override | `Configure().WithTimeout()` |
| `RetryPolicy` property override | `Configure().WithRetryCount()` |
| `ShouldSkip()` method | `Configure().WithSkipWhen()` |
| `ShouldIgnoreFailures()` method | `Configure().WithIgnoreFailures()` |
| `ModuleRunType.AlwaysRun` | `Configure().WithAlwaysRun()` |
| `OnBeforeExecute()` | `Configure().WithBeforeExecute()` or `OnBeforeExecuteAsync()` |
| `OnAfterExecute()` | `Configure().WithAfterExecute()` or `OnAfterExecuteAsync()` |

### Result Access

| V2 | V3 |
|----|-----|
| `result.Value` | `result.ValueOrDefault` or pattern match |
| `result.Exception` | `result.ExceptionOrDefault` or pattern match |
| `result.ModuleResultType == ModuleResultType.Success` | `result.IsSuccess` or pattern match |

### Removed Types

- `PipelineHostBuilder` - Use `Pipeline.CreateBuilder()`
- `ModuleBase` / `ModuleBase<T>` - Use `Module<T>`
- `Module` (non-generic) - Use `Module<IDictionary<string, object>>`

## Migration Path

### Quick Migration (Minimal Changes)

The `ExecutePipelineAsync()` extension still exists:

```csharp
var builder = Pipeline.CreateBuilder(args);
builder.Services.AddModule<MyModule>();
await builder.ExecutePipelineAsync();  // Still works
```

And `ValueOrDefault` provides backwards-compatible result access:

```csharp
var result = await context.GetModule<BuildModule>();
var value = result.ValueOrDefault;  // Similar to old result.Value
```

### Full Migration

For the cleanest code, adopt the new patterns:

1. Use `Pipeline.CreateBuilder(args)` with direct property access
2. Move module configuration to `Configure()` builder
3. Change `IPipelineContext` to `IModuleContext`
4. Move `GetModule<T>()` calls to context
5. Use pattern matching for result handling

See the [Migration Guide](https://thomhurst.github.io/ModularPipelines/how-to/migrating-to-v3) for detailed examples.

## Upgrade Steps

1. Update the NuGet package: `dotnet add package ModularPipelines --version 3.0.0`
2. Fix compile errors using the migration tables above
3. (Optional) Refactor to use new fluent APIs
4. (Optional) Adopt new features like tags, categories, and conditional attributes

## Getting Help

- [Full Migration Guide](https://thomhurst.github.io/ModularPipelines/how-to/migrating-to-v3)
- [GitHub Issues](https://github.com/thomhurst/ModularPipelines/issues) - Use the `migration` label
- [Examples](https://github.com/thomhurst/ModularPipelines/tree/main/src/ModularPipelines.Examples)
