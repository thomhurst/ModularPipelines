---
title: Logging
sidebar_position: 8
---

# Logging

## ILogger
When logging in a module, use the `ILogger` exposed by `context.Logger`. Do not inject a category logger such as `ILogger<T>` into a module, and do not write directly through `Console` or another console API.

The context logger applies Modular Pipelines' secret masking and groups output with the current module while still using the standard Microsoft.Extensions.Logging API.

These are detailed below.

## Secret Obfuscation
If secrets have been defined (See [Secrets](secrets) for details on this), then if any of them are attempted to be written through the context logger, either directly or through HTTP and command logs, they will be obfuscated in the output. For example, a bearer token of 'MySuperSecretToken' will appear as '**********'.

## Grouped Logs
When writing through `context.Logger`, logs are grouped by the current module.
Since all modules attempt to run in parallel, if there was no log organisation, then logs would be everywhere and all jumbled up, and hard to navigate. This keeps logs together, clean, and easy to read. This is why it's very important not to write to the console directly, as that'll prevent this from working.

## Interfering with Console Progress 
If you have an interactive terminal, then a progress dialog will be displayed, and constantly updated with the progress of all your modules.
If you start writing to the console directly, then you'll be writing over the top of this progress dialog and messing up how it renders.

## Analyzers
If you forget the above, Modular Pipelines has analyzers built as part of its framework. It'll detect direct uses of `Console`, or trying to inject in custom `ILogger`s and will result in errors, asking you to fix the issues.

## How to access ILogger

### Module
If you're in a module, it's part of your `context` object. Call `context.Logger`.

### Elsewhere
If you're in another class, inject `IModuleLoggerAccessor` and use its `Logger` property.

## Command Logging

When you execute CLI commands (e.g., `dotnet build`, `docker run`), ModularPipelines logs the command execution details. You can control what gets logged using `CommandLoggingOptions`.

### Verbosity Levels

| Level | Description |
|-------|-------------|
| `Silent` | No output at all |
| `Minimal` | Only command input (no output/errors) |
| `Normal` | Input, output, and errors on failure (default) |
| `Detailed` | Above plus exit code and duration |
| `Diagnostic` | Everything including working directory and timestamps |

### Per-Command Configuration

```csharp
await context.Tools.DotNet.BuildAsync(
    new DotNetBuildOptions { Configuration = "Release" },
    new CommandExecutionOptions
    {
        Logging = new CommandLoggingOptions
        {
            Verbosity = CommandLogVerbosity.Detailed
        }
    });
```

### Global Defaults

Set default logging for all commands at the pipeline level:

```csharp
var builder = Pipeline.CreateBuilder(args);

// All commands will use Silent logging unless overridden
builder.ConfigureOptions(options => options with
{
    Commands = options.Commands with
    {
        Logging = CommandLoggingOptions.Silent,
    },
});

// Or use Diagnostic for debugging
builder.ConfigureOptions(options => options with
{
    Commands = options.Commands with
    {
        Logging = CommandLoggingOptions.Diagnostic,
    },
});

await builder.RunAsync();
```

Configure Microsoft.Extensions.Logging providers and filters through the builder's
logging surface:

```csharp
builder.Logging
    .ClearProviders()
    .AddConsole()
    .SetMinimumLevel(LogLevel.Information);
```

### Using Presets

```csharp
// Silent - no command logging
new CommandExecutionOptions { Logging = CommandLoggingOptions.Silent }

// Diagnostic - maximum verbosity
new CommandExecutionOptions { Logging = CommandLoggingOptions.Diagnostic }

// Default - normal verbosity
new CommandExecutionOptions { Logging = CommandLoggingOptions.Default }
```

### Fine-Grained Control

Override individual settings regardless of verbosity level:

```csharp
new CommandLoggingOptions
{
    Verbosity = CommandLogVerbosity.Normal,
    ShowCommandArguments = true,
    ShowStandardOutput = true,
    ShowStandardError = true,
    ShowExitCode = true,
    ShowExecutionTime = true,
    ShowWorkingDirectory = false,
    ShowTimestamps = false
}
```

### Output Manipulators

Transform logged content before it's written (useful for truncating large outputs or redacting sensitive data):

```csharp
new CommandExecutionOptions
{
    Logging = new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Normal },
    InputLoggingManipulator = input => input.Length > 500
        ? input.Substring(0, 500) + "... (truncated)"
        : input,
    OutputLoggingManipulator = output => output.Replace("api-key-value", "***")
}
```

### Configuration Precedence

Logging settings are resolved in this order (highest to lowest priority):

1. **Per-Call**: `CommandExecutionOptions.Logging` on individual command calls
2. **Global Default**: `Commands.Logging` configured at pipeline level
3. **System Default**: `CommandLoggingOptions.Default` (Normal verbosity)

## HTTP Logging

HTTP logging uses the same `Logging` name at both scopes. Per-request options override
the pipeline default:

```csharp
await context.Network.Http.SendAsync(new HttpOptions(request)
{
    Logging = HttpLoggingOptions.Minimal,
});

builder.ConfigurePipelineOptions(options => options with
{
    Http = options.Http with
    {
        Logging = HttpLoggingOptions.None,
    },
});
```

Use `HttpLoggingOptions` properties for fine-grained request, response, status-code,
duration, header, and body logging. `HttpLoggingType` no longer exists.
