---
title: Node.js Package
---

# Node.js Package

Node.js, npm, npx, nvm, and strongly typed pnpm helpers.

## Installation

```shell
dotnet add package ModularPipelines.Node
```

Required command-line tools: `node`, `npm`, `npx`, `nvm`, and `pnpm`. Install the tools used by your pipeline and make them available on `PATH`.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Node`
- `context.Tools.Npm`
- `context.Tools.Npx`
- `context.Tools.Node.Nvm`
- `context.Tools.Pnpm`

## Module example

```csharp
using ModularPipelines;

public class UseNodeModule : Module<CommandResult>
{
    protected override async Task<CommandResult> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Tools.Node.VersionAsync(cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

## npm and npx in v4

npm and npx are generated from the installed npm CLI help, like pnpm. Import options
from `ModularPipelines.Node.Options` and service interfaces from
`ModularPipelines.Node.Services`. The handwritten npm/npx types in
`ModularPipelines.Node.Models` are removed. Node and nvm retain their existing APIs;
`context.Tools.Node.Npm` and `.Npx` also resolve the generated services.

Generated methods accept command options, optional `CommandExecutionOptions`, and a
`CancellationToken`. Use a named `cancellationToken` argument when migrating an old
two-argument call:

```csharp
await context.Tools.Npm.InstallAsync(
    new NpmInstallOptions
    {
        PackageSpec = ["typescript"],
        SaveDev = true,
    },
    cancellationToken: cancellationToken);

await context.Tools.Npx.ExecuteAsync(
    new NpxExecuteOptions
    {
        Package = ["typescript"],
        Pkg = "tsc",
        Args = ["--noEmit"],
    },
    cancellationToken: cancellationToken);
```

Subcommands use generated service groups: for example, replace `npm.OrgLsAsync(...)`
with `npm.Org.LsAsync(...)`, and `npm.TokenRevokeAsync(...)` with
`npm.Token.RevokeAsync(...)`. Required operands follow the current help synopsis;
collections use the generated collection type rather than the old handwritten shape.

Replace `NpmExecCOptions.Cmd` with `NpmExecOptions.Call`, and `NpxCOptions.Cmd` with
`NpxExecuteOptions.Call`. For direct execution, use `Pkg` and `Args`; npm options render
before the `--` separator. `NpmTeamCreateOptions` takes one `scope:team` operand, with
`Otp` supplied as an option instead of a second positional argument.

The profile 2FA options now use generated names: replace `NpmProfileDisable2faOptions`
with `NpmProfileDisable_2faOptions` and `NpmProfileEnable2faOptions` with
`NpmProfileEnable_2faOptions`, both in `ModularPipelines.Node.Options`.

See the generated [npm reference](cli/npm.md), [npx reference](cli/npx.md), and
[pnpm reference](cli/pnpm.md) for the current command surface.
