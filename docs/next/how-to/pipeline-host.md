# Pipeline Host

To begin creating your pipeline, use the `Pipeline.CreateBuilder()` method.

The recommended approach is to create a console application and use this builder in your `Program.cs` file.

The pipeline builder follows the ASP.NET Core minimal API pattern, providing direct access to:

* `Configuration` - for adding configuration sources
* `Services` - for dependency injection
* `Options` - for pipeline behavior settings
* `Environment` - for host environment information

## Basic Example[​](#basic-example "Direct link to Basic Example")

```
var builder = Pipeline.CreateBuilder(args);



builder

    .AddModule<BuildModule>()

    .AddModule<TestModule>()

    .AddModule<DeployModule>();



await builder.RunAsync();
```

Passing `args` also enables the [built-in pipeline command line](/ModularPipelines/docs/next/how-to/command-line.md) for listing, selecting, skipping, and validating modules.

## Configuration[​](#configuration "Direct link to Configuration")

Add configuration sources directly via the `Configuration` property:

```
var builder = Pipeline.CreateBuilder(args);



builder.Configuration

    .AddJsonFile("appsettings.json", optional: false)

    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)

    .AddUserSecrets<Program>()

    .AddEnvironmentVariables();



// Use configuration in services

builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));
```

## Registering Modules[​](#registering-modules "Direct link to Registering Modules")

Modules are registered directly on `PipelineBuilder`. Every registration method returns the same builder for chaining:

```
var builder = Pipeline.CreateBuilder(args);



// Register modules

builder

    .AddModule<Module1>()

    .AddModule<Module2>()

    .AddModule<Module3>();



// Or register multiple at once

builder.AddModules(typeof(Module1), typeof(Module2), typeof(Module3));
```

### Conditional Registration[​](#conditional-registration "Direct link to Conditional Registration")

Use `builder.Environment` or `builder.Configuration` for conditional module registration:

```
var builder = Pipeline.CreateBuilder(args);



builder.Configuration.AddJsonFile("appsettings.json");



// Environment-based registration

if (builder.Environment.IsDevelopment())

{

    builder.AddModule<DevOnlyModule>();

}



// Configuration-based registration

if (builder.Configuration.GetValue<bool>("EnableExtraModules"))

{

    builder.AddModule<OptionalModule>();

}



builder.AddModule<AlwaysRunModule>();
```

## Pipeline Options[​](#pipeline-options "Direct link to Pipeline Options")

Configure pipeline behavior via the `Options` property:

```
var builder = Pipeline.CreateBuilder(args);



builder.ConfigurePipelineOptions(options => options with

{

    // Failure mode

    FailureMode = FailureMode.FailFast,



    // Category filtering

    RunOnlyCategories = ["Build", "Test"],

    IgnoreCategories = ["Experimental"],



    // Display options

    Console = options.Console with

    {

        ShowProgress = true,

        PrintResults = true,

        PrintLogo = true,

    },



    // Concurrency settings

    Concurrency = new ConcurrencyOptions

    {

        MaxParallelism = 4,

    },

});
```

## Building and Running[​](#building-and-running "Direct link to Building and Running")

The pipeline follows a two-step build-then-run pattern:

```
var builder = Pipeline.CreateBuilder(args);

builder.ConfigurePipelineOptions(options => options with

{

    FailureMode = FailureMode.ContinueOnFailure,

    ThrowOnPipelineFailure = false,

});

builder.AddModule<MyModule>();



// Step 1: Build and validate the pipeline

await using var pipeline = await builder.BuildAsync();



// Step 2: Run it

var summary = await pipeline.RunAsync();



// Check results

if (summary.Status == ModularPipelines.Enums.ModuleStatus.Failed)

{

    Environment.Exit(1);

}
```

Use `ContinueOnFailure` with `ThrowOnPipelineFailure = false` when you need to inspect the returned summary after a module fails. Fail-fast mode rethrows the module exception.

`BuildAsync()` always validates the pipeline configuration before returning:

```
var builder = Pipeline.CreateBuilder(args);

builder.AddModule<MyModule>();



try

{

    // BuildAsync validates and throws PipelineValidationException on errors

    await using var pipeline = await builder.BuildAsync();

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

### Validate Without Running[​](#validate-without-running "Direct link to Validate Without Running")

```
var builder = Pipeline.CreateBuilder(args);

builder.AddModule<MyModule>();



var validation = await builder.ValidateAsync();

if (validation.HasErrors)

{

    foreach (var error in validation.Errors)

    {

        Console.WriteLine($"Validation Error: {error.Message}");

    }

    Environment.Exit(1);

}



await builder.RunAsync();
```

## Complete Example[​](#complete-example "Direct link to Complete Example")

```
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

builder.ConfigurePipelineOptions(options => options with

{

    FailureMode = FailureMode.FailFast,

    IgnoreCategories = ["Experimental"],

});



// Services

builder.Services.Configure<NuGetSettings>(builder.Configuration.GetSection("NuGet"));

builder.Services.Configure<PublishSettings>(builder.Configuration.GetSection("Publish"));



// Conditional modules

if (builder.Environment.IsDevelopment())

{

    builder

        .AddModule<LocalBuildModule>()

        .AddModule<LocalTestModule>();

}

else

{

    builder

        .AddModule<CIBuildModule>()

        .AddModule<CITestModule>()

        .AddModule<PublishModule>();

}



// Always-registered modules

builder

    .AddModule<CleanupModule>()

    .AddModule<ReportModule>();



// Run

await builder.RunAsync();
```

## Event Handlers and Requirements[​](#event-handlers-and-requirements "Direct link to Event Handlers and Requirements")

Register event handlers and pipeline requirements:

```
var builder = Pipeline.CreateBuilder(args);



// Pipeline event handlers (run before/after all modules)

builder.AddPipelineEventHandler<MyPipelineEventHandler>();



// Module event handlers (observe every module)

builder.AddModuleEventHandler<MyModuleEventHandler>();



// Requirements (validated before pipeline starts)

builder.AddRequirement<DotNetSdkRequirement>();

builder.AddRequirement<GitRequirement>();
```

## Extension Methods[​](#extension-methods "Direct link to Extension Methods")

For a more fluent API, extension methods are available:

```
var builder = Pipeline.CreateBuilder(args);



await builder

    .AddModule<Module1>()

    .AddModule<Module2>()

    .ConfigurePipelineOptions(options => options with

    {

        FailureMode = FailureMode.FailFast,

    })

    .RunAsync();
```
