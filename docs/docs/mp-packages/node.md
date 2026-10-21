---
title: Node.js Package
---

# Node.js Package

Node.js, npm, npx, nvm, and strongly typed pnpm helpers.

## Installation

```shell
dotnet add package ModularPipelines.Node
```

Required command-line tools: `node`, `npm`, `npx`, `nvm`, and `pnpm`. Install the tools used by your pipeline and make executables available on `PATH`. On Linux/macOS, nvm-sh is a shell function; the wrapper loads it as described below.

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
`ModularPipelines.Node.Models` are removed. Node retains its existing API;
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
with `NpmProfileDisableNumber2faOptions` and `NpmProfileEnable2faOptions` with
`NpmProfileEnableNumber2faOptions`, both in `ModularPipelines.Node.Options`.

`ShrinkwrapAsync`, `HelpAsync`, and `CompletionAsync` remain on the generated npm
service. Their option types move from `ModularPipelines.Node.Models` to
`ModularPipelines.Node.Options`. `NpmHelpOptions` now requires the first help term
in its constructor; additional terms use `Terms`.

See the generated [npm reference](cli/npm.md), [npx reference](cli/npx.md), and
[pnpm reference](cli/pnpm.md) for the current command surface.

## nvm behavior

On Linux/macOS, each nvm call starts Bash and sources `nvm.sh` from `NVM_DIR`.
When `NVM_DIR` is unset or empty, it uses `$XDG_CONFIG_HOME/nvm` if
`XDG_CONFIG_HOME` is set, otherwise `$HOME/.nvm`. Bash must be on `PATH`.
Arguments are passed separately to Bash, so spaces and shell metacharacters are
preserved. Loading nvm does not automatically select a default Node version.

On Windows, the wrapper runs the nvm-windows executable directly. `InstallAsync`
and `UseAsync` invoke its supported `install` and `use` commands. Selecting a
version changes the shared nvm-windows symlink, can require elevation, and can
affect other processes; avoid concurrent version selection.

`VersionAsync` now consistently reports **nvm's version** (`nvm --version` on
Unix, `nvm version` on Windows). Use `CurrentAsync` to report the active Node
version (`nvm current`). The unsupported cross-platform `WhichAsync` member is
removed; `CurrentAsync` returns a version, not an executable path.

`UseAsync` on Unix changes only its child shell. It cannot update the pipeline's
`PATH`, and a later `Node.VersionAsync`, npm, or npx call will not inherit the
selection. `InstallAsync` persists installed files, but its shell environment
changes are also temporary. To run commands using a selected Unix version, source
nvm, select the version, and run those commands in a single `context.Shell.Bash`
script, or configure their executable paths/environment explicitly.
