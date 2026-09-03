# Logging

## ILogger[​](#ilogger "Direct link to ILogger")

When logging in a module, use the `ILogger` exposed by `context.Logger`. Do not inject a category logger such as `ILogger<T>` into a module.

The context logger applies Modular Pipelines' secret masking and groups output with the current module while still using the standard Microsoft.Extensions.Logging API.

These are detailed below.

## Secret Obfuscation[​](#secret-obfuscation "Direct link to Secret Obfuscation")

If secrets have been defined (See [Secrets](/ModularPipelines/docs/next/how-to/secrets.md) for details on this), then if any of them are attempted to be written through the context logger, either directly or through HTTP and command logs, they will be obfuscated in the output. For example, a bearer token of 'MySuperSecretToken' will appear as '\*\*\*\*\*\*\*\*\*\*'.

## Grouped Logs[​](#grouped-logs "Direct link to Grouped Logs")

When writing through `context.Logger`, logs are grouped by the current module. Since modules may run in parallel, grouping keeps each module's structured, rich, and plain console output together and readable.

## Interfering with Console Progress[​](#interfering-with-console-progress "Direct link to Interfering with Console Progress")

If you have an interactive terminal, then a progress dialog will be displayed, and constantly updated with the progress of all your modules. Module output is buffered while this display is active so it does not overwrite the progress dialog.

## Analyzers[​](#analyzers "Direct link to Analyzers")

Modular Pipelines analyzers recommend `context.Console` over direct `Console` calls and prevent injecting category loggers into modules. Direct stdout/stderr remains a supported capture path when console stream semantics are specifically required.

## How to access ILogger[​](#how-to-access-ilogger "Direct link to How to access ILogger")

### Module[​](#module "Direct link to Module")

If you're in a module, it's part of your `context` object. Call `context.Logger`.

### Elsewhere[​](#elsewhere "Direct link to Elsewhere")

If you're in another class, inject `IModuleLoggerAccessor` and use its `Logger` property.

## Choosing an output API[​](#choosing-an-output-api "Direct link to Choosing an output API")

Modular Pipelines keeps all three module output forms in one ordered module group:

* Use `System.Console.Write*` and `System.Console.Error.Write*` for captured plain stdout and stderr with normal console fragment and newline semantics.
* Use `context.Console` for captured plain text or Spectre.Console rendering.
* Use `context.Logger` for structured, levelled events delivered to the configured Microsoft.Extensions.Logging providers and filters.

Raw console writes are not converted into `Information` or `Error` log events. They are not sent to non-console logging providers and logging filters do not suppress them. Secrets are masked before console rendering and optional run-report capture, including secrets split across several `Write` calls.

```
context.Console.WriteLine("Literal [brackets] stay literal");

context.Console.WriteMarkupLine("[green]Build succeeded[/]");

context.Console.Write(new Table().AddColumn("Result").AddRow("Succeeded"));
```

Attribution follows the .NET `ExecutionContext`, so it survives normal `await` continuations and `Task.Run`. It also applies during module construction, nested submodules, and distributed execution. Concurrent modules retain separate partial-line state for stdout and stderr, so fragments and lines cannot migrate between module groups.

Writes made with `ExecutionContext.SuppressFlow()` cannot be attributed and use the pipeline/unattributed output group. The same is true for fire-and-forget work that writes after its owning module's output scope has ended. If that work continues after the pipeline restores the process console, ordinary process-console behavior applies. Await module-owned work whenever its output must belong to that module.

Capture and grouping remain active when `Console.ShowProgress` is `false`. Rendering of the resulting groups still follows the selected local or CI build-system formatter. Avoid using the global `AnsiConsole` directly; use `context.Console` for rich output so the correct module buffer, ordering, and masking rules apply.

## Command Logging[​](#command-logging "Direct link to Command Logging")

When you execute CLI commands (e.g., `dotnet build`, `docker run`), ModularPipelines logs the command execution details. You can control what gets logged using `CommandLoggingOptions`.

### Verbosity Levels[​](#verbosity-levels "Direct link to Verbosity Levels")

| Level        | Description                                           |
| ------------ | ----------------------------------------------------- |
| `Silent`     | No output at all                                      |
| `Minimal`    | Only command input (no output/errors)                 |
| `Normal`     | Input, output, and errors on failure (default)        |
| `Detailed`   | Above plus exit code and duration                     |
| `Diagnostic` | Everything including working directory and timestamps |

### Per-Command Configuration[​](#per-command-configuration "Direct link to Per-Command Configuration")

```
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

### Global Defaults[​](#global-defaults "Direct link to Global Defaults")

Set default logging for all commands at the pipeline level:

```
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

Configure Microsoft.Extensions.Logging providers and filters through the builder's logging surface:

```
builder.Logging

    .ClearProviders()

    .AddConsole()

    .SetMinimumLevel(LogLevel.Information);
```

### Using Presets[​](#using-presets "Direct link to Using Presets")

```
// Silent - no command logging

new CommandExecutionOptions { Logging = CommandLoggingOptions.Silent }



// Diagnostic - maximum verbosity

new CommandExecutionOptions { Logging = CommandLoggingOptions.Diagnostic }



// Default - normal verbosity

new CommandExecutionOptions { Logging = CommandLoggingOptions.Default }
```

### Fine-Grained Control[​](#fine-grained-control "Direct link to Fine-Grained Control")

Override individual settings regardless of verbosity level:

```
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

### Output Manipulators[​](#output-manipulators "Direct link to Output Manipulators")

Transform logged content before it's written (useful for truncating large outputs or redacting sensitive data):

```
new CommandExecutionOptions

{

    Logging = new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Normal },

    InputLoggingManipulator = input => input.Length > 500

        ? input.Substring(0, 500) + "... (truncated)"

        : input,

    OutputLoggingManipulator = output => output.Replace("api-key-value", "***")

}
```

### Configuration Precedence[​](#configuration-precedence "Direct link to Configuration Precedence")

Logging settings are resolved in this order (highest to lowest priority):

1. **Per-Call**: `CommandExecutionOptions.Logging` on individual command calls
2. **Global Default**: `Commands.Logging` configured at pipeline level
3. **System Default**: `CommandLoggingOptions.Default` (Normal verbosity)

## HTTP Logging[​](#http-logging "Direct link to HTTP Logging")

HTTP logging uses the same `Logging` name at both scopes. Per-request options override the pipeline default:

```
await context.Network.Http.SendAsync(new HttpOptions(request)

{

    Logging = HttpLoggingOptions.Minimal,

});



builder.ConfigureOptions(options => options with

{

    Http = options.Http with

    {

        Logging = HttpLoggingOptions.None,

    },

});
```

Use `HttpLoggingOptions` properties for fine-grained request, response, status-code, duration, header, and body logging. `HttpLoggingType` no longer exists.
