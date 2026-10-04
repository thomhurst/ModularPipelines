---
title: WinGet Package
---

# WinGet Package

Strongly typed Windows Package Manager commands.

## Common options

The common-option audit uses WinGet v1.29.380 help and Microsoft's
[WinGet reference](https://learn.microsoft.com/en-us/windows/package-manager/winget/).
`WingetOptions` supplies `Wait`, `Logs`, `Verbose`, `Nowarn`,
`DisableInteractivity`, `Proxy`, and `NoProxy` to every command. These options
follow the command: for example, `winget search --query example --verbose`.
WinGet rejects a command placed after root options.

Existing command initializers can continue setting inherited properties.
Options with multiple documented names now generate one property using the first
long name; for example, `--logs,--open-logs` becomes `Logs`. Root information
operations (`--version`, `--info`, and help) are not inherited. Command-specific
options retain their own value shape and repetition rules.

## Installation

```shell
dotnet add package ModularPipelines.WinGet
```

Required command-line tool: `winget`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Winget`

## Module example

```csharp

public class UseWingetModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var winget = context.Tools.Winget;

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Winget integration is ready");
        return None.Value;
    }
}
```

The package exposes generated options records for its supported CLI commands.
