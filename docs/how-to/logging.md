# Logging

## IModuleLogger[​](#imodulelogger "Direct link to IModuleLogger")

When logging in your pipeline, you should always use an IModuleLogger. Do not inject in your own ILogger, and do not write to the console directly using the `Console` class, or any other class that would write directly to the console.

This is because the `IModuleLogger` will perform many actions that will improve the output of your logs.

These are detailed below.

## Secret Obfuscation[​](#secret-obfuscation "Direct link to Secret Obfuscation")

If secrets have been defined (See [Secrets](/ModularPipelines/docs/how-to/secrets.md) for details on this), then if any of them are attempted to be written to logs via IModuleLogger, either directly, or through things like HTTP or Command logs, they will be obfuscated in the output. So for instance, if you made a HTTP call with a bearer token of 'MySuperSecretToken', when the HTTP request is logged, that bearer will show as '\*\*\*\*\*\*\*\*\*\*'.

## Grouped Logs[​](#grouped-logs "Direct link to Grouped Logs")

When writing logs through IModuleLogger, logs will be grouped by what module they're in. Since all modules attempt to run in parallel, if there was no log organisation, then logs would be everywhere and all jumbled up, and hard to navigate. This keeps logs together, clean, and easy to read. This is why it's very important not to write to the console directly, as that'll prevent this from working.

## Interfering with Console Progress[​](#interfering-with-console-progress "Direct link to Interfering with Console Progress")

If you have an interactive terminal, then a progress dialog will be displayed, and constantly updated with the progress of all your modules. If you start writing to the console directly, then you'll be writing over the top of this progress dialog and messing up how it renders.

## Analyzers[​](#analyzers "Direct link to Analyzers")

If you forget the above, Modular Pipelines has analyzers built as part of its framework. It'll detect direct uses of `Console`, or trying to inject in custom `ILogger`s and will result in errors, asking you to fix the issues.

## How to access IModuleLogger[​](#how-to-access-imodulelogger "Direct link to How to access IModuleLogger")

### Module[​](#module "Direct link to Module")

If you're in a module, it's part of your `context` object. Call `context.Logger`.

### Elsewhere[​](#elsewhere "Direct link to Elsewhere")

If you're in another class, you can inject in `IModuleLoggerProvider` and call `GetLogger`.

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
await context.DotNet().Build(

    new DotNetBuildOptions { Configuration = "Release" },

    new CommandExecutionOptions

    {

        LogSettings = new CommandLoggingOptions

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

builder.Options.DefaultLoggingOptions = CommandLoggingOptions.Silent;



// Or use Diagnostic for debugging

builder.Options.DefaultLoggingOptions = CommandLoggingOptions.Diagnostic;



await builder.Build().RunAsync();
```

### Using Presets[​](#using-presets "Direct link to Using Presets")

```
// Silent - no command logging

new CommandExecutionOptions { LogSettings = CommandLoggingOptions.Silent }



// Diagnostic - maximum verbosity

new CommandExecutionOptions { LogSettings = CommandLoggingOptions.Diagnostic }



// Default - normal verbosity

new CommandExecutionOptions { LogSettings = CommandLoggingOptions.Default }
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

    IncludeTimestamps = false

}
```

### Output Manipulators[​](#output-manipulators "Direct link to Output Manipulators")

Transform logged content before it's written (useful for truncating large outputs or redacting sensitive data):

```
new CommandExecutionOptions

{

    LogSettings = new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Normal },

    InputLoggingManipulator = input => input.Length > 500

        ? input.Substring(0, 500) + "... (truncated)"

        : input,

    OutputLoggingManipulator = output => output.Replace("api-key-value", "***")

}
```

### Configuration Precedence[​](#configuration-precedence "Direct link to Configuration Precedence")

Logging settings are resolved in this order (highest to lowest priority):

1. **Per-Call**: `CommandExecutionOptions.LogSettings` on individual command calls
2. **Global Default**: `builder.Options.DefaultLoggingOptions` set at pipeline level
3. **System Default**: `CommandLoggingOptions.Default` (Normal verbosity)
