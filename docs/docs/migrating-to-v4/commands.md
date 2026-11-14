---
title: Tools and commands
sidebar_position: 8
---

# Tools and commands

## Tool access

```csharp
// V3
await context.DotNet().Build(
    new DotNetBuildOptions { Configuration = "Release" },
    new CommandExecutionOptions { LogSettings = CommandLoggingOptions.Silent },
    cancellationToken: cancellationToken);

// V4
await context.Tools.DotNet.BuildAsync(
    new DotNetBuildOptions { Configuration = "Release" },
    new CommandExecutionOptions { Logging = CommandLoggingOptions.Silent },
    cancellationToken: cancellationToken);
```

Three things changed:

1. **`context.X()` becomes `context.Tools.X`.** This applies to Git, Docker, cloud CLIs, and every
   other integration. For example, `context.Git().Information` becomes
   `context.Tools.Git.Information`.
2. **Task-returning command methods end in `Async`.**
3. **`LogSettings` becomes `Logging`.** See [Logging and HTTP](./logging-and-http.md).

Notes:

- Add `using ModularPipelines.Context;`. The `Tools` properties are C# 14 extension members in
  that namespace. The old per-integration extension imports are no longer needed.
- On C# 13 or another .NET language, resolve the service with
  `context.Tools.Get<ModularPipelines.DotNet.Services.IDotNet>()`.
- Rename `token:` to `cancellationToken:` only where it names a changed ModularPipelines parameter.
- Some overloads accept a token straight after the tool options. A named argument makes the
  intended overload clear.

The option types themselves were regenerated. See [CLI options](./cli-options.md). Git has further
changes. See [Integration packages](./integrations.md#git).

## Raw commands and scripts

| V3 API | V4 API |
| --- | --- |
| `context.Shell.Command.ExecuteCommandLineTool(options, executionOptions, token)` | `context.Shell.RunAsync(options, executionOptions, token)` |
| `GenericCommandLineToolOptions`; the fluent `ICommandBuilder` / `CommandBuilderBase<,>` types in `ModularPipelines.Builders`; `DotNetBuildBuilder` | `new CommandLineToolOptions("tool")`, the tool's options record, or the raw `RunAsync` overload |
| `context.Shell.Bash.Command(options, token)` | `context.Shell.Bash.RunAsync(options, executionOptions, token)` |
| `context.Shell.Bash.FromFile(options, token)` | `context.Shell.Bash.RunFileAsync(options, executionOptions, token)` |
| `context.Shell.PowerShell.Script(options, token)` | `context.Shell.PowerShell.RunAsync(options, executionOptions, token)` |
| `context.Shell.PowerShell.FromFile(options, token)` | `context.Shell.PowerShell.RunFileAsync(options, executionOptions, token)` |
| `PowershellOptions`, `PowershellScriptOptions`, `PowershellFileOptions` | `PowerShellOptions`, `PowerShellScriptOptions`, `PowerShellFileOptions` |
| `context.Cmd().Script(options)` | `context.Tools.Cmd.RunAsync(options)`, or `RunFileAsync` for script files |

Pass raw arguments as separate tokens:

```csharp
await context.Shell.RunAsync(
    "dotnet", ["build", "My App.csproj", "--configuration", "Release"],
    cancellationToken: cancellationToken);
```

:::warning Positional tokens no longer compile
Script methods now take `CommandExecutionOptions` as the second parameter, like tool integrations.
A V3 call that passed the token positionally, such as `Bash.Command(options, token)`, needs
`cancellationToken: token` after the rename. The same applies to `Tools.Cmd.RunAsync`.
:::

For Command Prompt, `ICmd` is replaced by `ICmdContext`, and `CmdScriptOptions` moved from
`ModularPipelines.Cmd.Models` to `ModularPipelines.Options`.

Custom integration implementations can still use `ICommandContext.ExecuteCommandLineToolAsync`.
`Shell.RunAsync` is the convenience API for modules.

## Execution options and results

- **Environment variables.** `CommandExecutionOptions.EnvironmentVariables` is now
  `IReadOnlyDictionary<string, string?>`. Build the dictionary before you assign it, rather than
  mutating it afterwards.
- **`CommandExecutionOptions.ToCommandLineToolOptions(...)`** is removed.
- **CliWrap types** are no longer part of the public contract. Use
  `ModularPipelines.Options.CommandLineCredentials` for command credentials.
- **Synthetic results.** In tests or interceptors, use the new `CommandResult.Ok("output")` or the
  explicit constructor.
- **Expected nonzero exit codes.** Set `CommandExecutionOptions.ThrowOnNonZeroExitCode = false` and
  inspect the returned `ExitCode`.

`CommandException` changed shape. See
[Exceptions](./exceptions-and-extension-points.md#commandexception).

Commands also have a new default timeout and output capture limit. See
[Runtime behavior changes](./runtime-behavior.md#commands-and-processes).
