---
title: ShellCheck Package
---

# ShellCheck Package

The `ModularPipelines.Shellcheck` package provides strongly typed access to the ShellCheck static analysis CLI.

Install the package and invoke ShellCheck through the pipeline context:

```csharp
using ModularPipelines.Shellcheck.Enums;
using ModularPipelines.Shellcheck.Extensions;
using ModularPipelines.Shellcheck.Options;

public class CheckShellScriptsModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Shellcheck().Execute(
            new ShellcheckExecuteOptions(["scripts/build.sh", "scripts/deploy.sh"])
            {
                CheckSourced = true,
                Format = ShellcheckFormat.Gcc,
                Severity = ShellcheckSeverity.Warning,
                Shell = ShellcheckShell.Bash,
            },
            cancellationToken: cancellationToken);
    }
}
```

Input files are required constructor arguments. Options with fixed value sets use enums, `--extended-analysis` uses a boolean, and `--wiki-link-count` uses an integer. ShellCheck must be installed and available on `PATH` when the pipeline runs.
