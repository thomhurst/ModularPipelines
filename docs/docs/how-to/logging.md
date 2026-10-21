---
title: Logging
sidebar_position: 8
---

# Logging

## ILogger
When logging in a module, use the `ILogger` exposed by `context.Logger`. Do not inject a category logger such as `ILogger<T>` into a module.

The context logger applies Modular Pipelines' secret masking and groups output with the current module while still using the standard Microsoft.Extensions.Logging API.

These are detailed below.

## Secret Obfuscation
If secrets have been defined (See [Secrets](secrets) for details on this), then if any of them are attempted to be written through the context logger, either directly or through HTTP and command logs, they will be obfuscated in the output. For example, a bearer token of 'MySuperSecretToken' will appear as '**********'.

## Grouped Logs
When writing through `context.Logger`, logs are grouped by the current module.
Since modules may run in parallel, grouping keeps each module's structured, rich, and
plain console output together and readable.

On build systems that support collapsible sections (GitHub Actions, Azure Pipelines, GitLab, TeamCity) and in the local console, each group is headed with the module name, its outcome and its duration, for example `BuildModule ✓ (12s 40ms)`. When the results table is printed, the separate "completed successfully" and "skipped" status lines are then not repeated on the console; other logging providers, such as file or telemetry sinks, still receive them at `Information`. Build systems without group headers, and runs with `Console.PrintResults` set to `false`, still show them on the console.

File system operations such as reading, writing, copying and searching files and folders are logged at `Debug` level.

## Summary Entries
Use `context.Summary` to record facts worth reading after the run, such as a generated version or a deployment URL:

```csharp
context.Summary.KeyValue("Version", "Generated Version Number", version);
context.Summary.Success("Deploy", "Deployed to production");
```

Entries appear in a **Summary** section of the final results output, grouped by category, with registered secrets masked. They are also written to every other configured logging provider (for example file or telemetry sinks) at the end of the run. If results are not printed (for example `Console.PrintResults` is `false`), the entries are written to the console logger instead.

## Interfering with Console Progress 
If you have an interactive terminal, then a progress dialog will be displayed, and constantly updated with the progress of all your modules.
Module output is buffered while this display is active so it does not overwrite the progress dialog.

## Analyzers
Modular Pipelines analyzers recommend `context.Console` over direct `Console` calls and
prevent injecting category loggers into modules. Direct stdout/stderr remains a supported
capture path when console stream semantics are specifically required.

## How to access ILogger

### Module
If you're in a module, it's part of your `context` object. Call `context.Logger`.

### Elsewhere
If you're in another class, inject `IModuleLoggerAccessor` and use its `Logger` property.

## Choosing an output API

Modular Pipelines keeps all three module output forms in one ordered module group:

- Use `System.Console.Write*` and `System.Console.Error.Write*` for captured plain stdout
  and stderr with normal console fragment and newline semantics.
- Use `context.Console` for captured plain text or Spectre.Console rendering.
- Use `context.Logger` for structured, levelled events delivered to the configured
  Microsoft.Extensions.Logging providers and filters.

Raw console writes are not converted into `Information` or `Error` log events. They are
not sent to non-console logging providers and logging filters do not suppress them.
Secrets are masked before console rendering and optional run-report capture, including
secrets split across several `Write` calls.

```csharp
context.Console.WriteLine("Literal [brackets] stay literal");
context.Console.WriteMarkupLine("[green]Build succeeded[/]");
context.Console.Write(new Table().AddColumn("Result").AddRow("Succeeded"));
```

Attribution follows the .NET `ExecutionContext`, so it survives normal `await`
continuations and `Task.Run`. It also applies during module construction, nested
submodules, and distributed execution. Concurrent modules retain separate partial-line
state for stdout and stderr, so fragments and lines cannot migrate between module groups.

Writes made with `ExecutionContext.SuppressFlow()` cannot be attributed and use the
pipeline/unattributed output group. The same is true for fire-and-forget work that writes
after its owning module's output scope has ended. If that work continues after the
pipeline restores the process console, ordinary process-console behavior applies. Await
module-owned work whenever its output must belong to that module.

Capture and grouping remain active when `Console.ShowProgress` is `false`. Rendering of
the resulting groups still follows the selected local or CI build-system formatter.
Avoid using the global `AnsiConsole` directly; use `context.Console` for rich output so
the correct module buffer, ordering, and masking rules apply.

## Command Logging

When you execute CLI commands (e.g., `dotnet build`, `docker run`), ModularPipelines logs the command execution details. You can control what gets logged using `CommandLoggingOptions`.

Command output is logged one line at a time, each prefixed with `↳`, whether it is streamed while the command runs or captured when it finishes. Blank output lines are omitted. Streamed standard error lines are logged the same way, because many tools write progress to standard error. When output is captured instead of streamed (for example with an output manipulator), standard error from a failed command is logged as a single warning prefixed with `✗`.

### Verbosity Levels

| Level | Description |
|-------|-------------|
| `Silent` | No output unless a `Show*` override explicitly enables it |
| `InputOnly` | Only command input (no output/errors) |
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
builder.ConfigureCommands(commands => commands with
{
    Logging = CommandLoggingOptions.Silent,
});

// Or use Diagnostic for debugging
builder.ConfigureCommands(commands => commands with
{
    Logging = CommandLoggingOptions.Diagnostic,
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

Each command `Show*` setting is nullable: `null` follows the verbosity level, `true` enables it,
and `false` disables it, including with `Silent` and `Diagnostic`. Presets set only verbosity,
so changing a preset's verbosity does not leave hidden overrides behind. `ShowCommandArguments = false`
masks command input; the command start and status markers can still be logged.

Working directories appear only at `Diagnostic` by default. `ShowWorkingDirectory` overrides that
choice. `ShowTimestamps` prefixes each command log event (start, streamed/captured output, and
completion) with its UTC logging time in ISO 8601 format. It does not recover timestamps from
already-captured process output. Both settings also apply to dry-run command starts.

With `ShowStandardError = null`, captured stderr is logged only on command failure; streamed
stderr is logged at `Normal` or above because tools also use it for progress. An explicit `true`
logs stderr even for successful captured commands; `false` disables both paths.

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

### Command Output Encoding

`CommandExecutionOptions.OutputEncoding` controls how standard output and standard error are
decoded. It defaults to `System.Text.Encoding.UTF8`. A byte order mark in either stream takes
precedence over this setting for that stream. Set it per command when a tool emits another encoding:

```csharp
var result = await context.Shell.RunAsync(
    "legacy-tool",
    ["export"],
    new CommandExecutionOptions { OutputEncoding = System.Text.Encoding.Latin1 },
    cancellationToken);
```

This controls decoding of process output, not the encoding a tool uses to write files.

### Captured Output Limits

`CommandExecutionOptions.MaxCapturedOutputLength` defaults to 1,048,576 characters
per stream. Longer output retains its beginning and end, with
`... [truncated N characters] ...` in the middle. This affects `CommandResult`
capture, not the complete streamed log output.

Check `StandardOutputTruncatedCharacters` and `StandardErrorTruncatedCharacters`
before parsing captured JSON or other structured output. Each count is zero when
that stream is complete. The counts are also available on `CommandException.Result`.
A warning names the limit and reports both counts when capture truncates output
and effective command verbosity is `Normal`, `Detailed`, or `Diagnostic`.
`Silent` and `InputOnly` suppress this warning; the result counts remain available.

Raise the limit for commands that produce large results, or use `0` for unlimited
capture (which retains all output in memory):

```csharp
var result = await context.Shell.RunAsync("kubectl", ["get", "pods", "-o", "json"],
    new CommandExecutionOptions { MaxCapturedOutputLength = 8 * 1024 * 1024 });

if (result.StandardOutputTruncatedCharacters > 0)
{
    throw new InvalidOperationException("Command output is incomplete; increase the capture limit.");
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

builder.ConfigureHttp(http => http with
{
    Logging = HttpLoggingOptions.Silent,
});
```

Use `HttpLoggingOptions` properties for fine-grained request, response, status-code,
duration, header, and body logging. Both command and HTTP logging use the `Show*` prefix and
`Silent`, `Default`, and `Diagnostic` presets. HTTP additionally provides `Minimal` (URL, status,
and duration) and `Headers` (no bodies). `Default` and a new `HttpLoggingOptions` instance omit
request and response bodies, since they can contain credentials or newly issued tokens that
have not been registered for secret masking. Request/response details, headers, status, and
duration are still logged.

To opt in to body logging, set `ShowRequestBody` and/or `ShowResponseBody` to `true` (with a
4 KiB limit by default), or select `Diagnostic` (both bodies, up to 64 KiB). Only registered
secret values are masked in bodies; enable body logging only when the content is safe to log.

HTTP failure exceptions omit response bodies from `Message` and `ToString()`, even with
`Diagnostic`. `PipelineHttpResponseException.ResponseContent` retains an unredacted preview
of up to 2,000 body bytes (plus a truncation marker) for programmatic inspection. Treat that
property as sensitive. To display a failure body, explicitly enable response body logging
with `ShowResponse = true` and `ShowResponseBody = true`, or select `Diagnostic`, so the
HTTP logger applies registered-secret masking.

HTTP flags remain `bool`, since HTTP has no verbosity level to inherit.
`HttpLoggingType` no longer exists.

```csharp
var logging = HttpLoggingOptions.Diagnostic with
{
    ShowRequestBody = false,
    ShowResponseBody = false,
    ShowDuration = true,
};
```

For earlier V4 builds, rename HTTP `LogRequest`, `LogResponse`, `LogRequestHeaders`,
`LogResponseHeaders`, `LogRequestBody`, `LogResponseBody`, `LogStatusCode`, and `LogDuration` to
`ShowRequest`, `ShowResponse`, `ShowRequestHeaders`, `ShowResponseHeaders`, `ShowRequestBody`,
`ShowResponseBody`, `ShowStatusCode`, and `ShowDuration`. Rename `HttpLoggingOptions.None` to
`Silent` and `HttpLoggingOptions.Full` to `Diagnostic`.
