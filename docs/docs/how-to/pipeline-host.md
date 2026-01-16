---
title: Pipeline Host
sidebar_position: 1
---

# Pipeline Host

To begin creating your pipeline, use the `Pipeline.CreateBuilder()` method.

The recommended approach is to create a console application and use this builder in your `Program.cs` file.

The pipeline builder follows the ASP.NET Core minimal API pattern, providing direct access to:
- `Configuration` - for adding configuration sources
- `Services` - for dependency injection
- `Options` - for pipeline behavior settings
- `Environment` - for host environment information

## Basic Example

```csharp
var builder = Pipeline.CreateBuilder(args);

builder.Services
    .AddModule<BuildModule>()
    .AddModule<TestModule>()
    .AddModule<DeployModule>();

await builder.Build().RunAsync();
```

## Configuration

Add configuration sources directly via the `Configuration` property:

```csharp
var builder = Pipeline.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

// Use configuration in services
builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));
```

## Registering Modules

Modules can be registered directly on the `Services` collection:

```csharp
var builder = Pipeline.CreateBuilder(args);

// Register modules
builder.Services
    .AddModule<Module1>()
    .AddModule<Module2>()
    .AddModule<Module3>();

// Or register multiple at once
builder.AddModules<Module1, Module2, Module3>();
```

### Conditional Registration

Use `builder.Environment` or `builder.Configuration` for conditional module registration:

```csharp
var builder = Pipeline.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

// Environment-based registration
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddModule<DevOnlyModule>();
}

// Configuration-based registration
if (builder.Configuration.GetValue<bool>("EnableExtraModules"))
{
    builder.Services.AddModule<OptionalModule>();
}

builder.Services.AddModule<AlwaysRunModule>();
```

## Pipeline Options

Configure pipeline behavior via the `Options` property:

```csharp
var builder = Pipeline.CreateBuilder(args);

// Execution mode
builder.Options.ExecutionMode = ExecutionMode.StopOnFirstException;

// Category filtering
builder.Options.RunOnlyCategories = ["Build", "Test"];
builder.Options.IgnoreCategories = ["Experimental"];

// Display options
builder.Options.ShowProgressInConsole = true;
builder.Options.PrintResults = true;
builder.Options.PrintLogo = true;

// Concurrency settings
builder.Options.Concurrency = new ConcurrencyOptions
{
    MaxParallelModules = 4
};
```

## Building and Running

The pipeline follows a two-step build-then-run pattern:

```csharp
var builder = Pipeline.CreateBuilder(args);
builder.Services.AddModule<MyModule>();

// Step 1: Build the pipeline
var pipeline = builder.Build();

// Step 2: Run it
var summary = await pipeline.RunAsync();

// Check results
if (summary.Status == PipelineStatus.Failed)
{
    Environment.Exit(1);
}
```

### With Validation

Use `BuildAsync()` to validate the pipeline configuration before running:

```csharp
var builder = Pipeline.CreateBuilder(args);
builder.Services.AddModule<MyModule>();

try
{
    // BuildAsync validates and throws PipelineValidationException on errors
    var pipeline = await builder.BuildAsync();
    await pipeline.RunAsync();
}
catch (PipelineValidationException ex)
{
    foreach (var error in ex.ValidationResult.Errors)
    {
        Console.WriteLine($"[{error.Category}] {error.Message}");
    }
    Environment.Exit(1);
}
```

### Validate Without Running

```csharp
var builder = Pipeline.CreateBuilder(args);
builder.Services.AddModule<MyModule>();

var validation = await builder.ValidateAsync();
if (validation.HasErrors)
{
    foreach (var error in validation.Errors)
    {
        Console.WriteLine($"Validation Error: {error.Message}");
    }
    Environment.Exit(1);
}

await builder.Build().RunAsync();
```

## Complete Example

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines;
using ModularPipelines.Extensions;
using ModularPipelines.Options;

var builder = Pipeline.CreateBuilder(args);

// Configuration
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

// Options
builder.Options.ExecutionMode = ExecutionMode.StopOnFirstException;
builder.Options.IgnoreCategories = ["Experimental"];

// Services
builder.Services.Configure<NuGetSettings>(builder.Configuration.GetSection("NuGet"));
builder.Services.Configure<PublishSettings>(builder.Configuration.GetSection("Publish"));

// Conditional modules
if (builder.Environment.IsDevelopment())
{
    builder.Services
        .AddModule<LocalBuildModule>()
        .AddModule<LocalTestModule>();
}
else
{
    builder.Services
        .AddModule<CIBuildModule>()
        .AddModule<CITestModule>()
        .AddModule<PublishModule>();
}

// Always-registered modules
builder.Services
    .AddModule<CleanupModule>()
    .AddModule<ReportModule>();

// Run
await builder.Build().RunAsync();
```

## Hooks and Requirements

Register global hooks and pipeline requirements:

```csharp
var builder = Pipeline.CreateBuilder(args);

// Global hooks (run before/after all modules)
builder.AddPipelineGlobalHooks<MyGlobalHooks>();

// Module hooks (run before/after each module)
builder.AddPipelineModuleHooks<MyModuleHooks>();

// Requirements (validated before pipeline starts)
builder.AddRequirement<DotNetSdkRequirement>();
builder.AddRequirement<GitRequirement>();
```

## Extension Methods

For a more fluent API, extension methods are available:

```csharp
var builder = Pipeline.CreateBuilder(args);

await builder
    .ConfigureServices(services =>
    {
        services.AddModule<Module1>();
        services.AddModule<Module2>();
    })
    .ConfigurePipelineOptions(options =>
    {
        options.ExecutionMode = ExecutionMode.StopOnFirstException;
    })
    .ExecutePipelineAsync();
```
