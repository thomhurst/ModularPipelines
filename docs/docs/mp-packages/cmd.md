---
title: Command Prompt Package
---

# Command Prompt Package

Helpers for executing Windows Command Prompt commands.

## Installation

```shell
dotnet add package ModularPipelines.Cmd
```

Required command-line tool: `cmd`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Cmd`

## Module example

```csharp

public class UseCmdModule : Module<CommandResult>
{
    protected override Task<CommandResult> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return context.Tools.Cmd.RunAsync("echo ModularPipelines", cancellationToken: cancellationToken);
    }
}
```

The integration implements `ICmdContext`. Inline scripts use `RunAsync`; batch
files use `RunFileAsync`. Both accept `CommandExecutionOptions`:

```csharp
await context.Tools.Cmd.RunAsync(
    new CmdScriptOptions("echo ModularPipelines"),
    new CommandExecutionOptions { ThrowOnNonZeroExitCode = true },
    cancellationToken);

await context.Tools.Cmd.RunFileAsync(
    new CmdFileOptions("scripts/build.cmd"),
    cancellationToken: cancellationToken);
```

The option records use the same `ModularPipelines.Options` namespace as the Bash
and PowerShell shell options.
