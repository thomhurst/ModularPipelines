---
title: Custom Commands
---

# Custom Commands
Many common CLI tools, such as npm, yarn, dotnet, docker, kubectl, have all had strong objects created to wrap around their CLI commands.

If you want to run a command that isn't currently supported by strong objects, run it directly through `context.Shell` within your modules.

Every argument should be passed as a separate string in a collection. This allows proper formatting if there's things like spaces or quotes.

## Example

```csharp
await context.Shell.RunAsync(
    "dotnet",
    ["tool", "install", "--global", "dotnet-coverage"],
    cancellationToken);
```

This is the equivalent to running:

`dotnet tool install --global dotnet-coverage`

By default, `Arguments` appears after generated non-terminal options and operands. It
appears before `RunSettings` (and its `--` marker) and before options in the `Terminal`
phase. When `ArgumentsContainToolOptions` is enabled, recognized tool options can be
hoisted ahead of a structured or declared end-of-options marker.

If a switch exists both globally and on the selected command, manual `Arguments`
use the command-local definition, including its value arity and short aliases.
Set the typed global property or use `AdditionalArguments` with `IsGlobalOption: true`
to place that switch before the subcommand explicitly.

When you need to configure the options object, construct `CommandLineToolOptions`
directly:

```csharp
await context.Shell.RunAsync(
    new CommandLineToolOptions("dotnet")
    {
        Arguments = ["tool", "restore"],
    },
    executionOptions,
    cancellationToken);
```

## Standard Input

Pass text to a command through `CommandExecutionOptions.StandardInput`:

```csharp
await context.Shell.RunAsync(
    "kubectl",
    ["apply", "-f", "-"],
    new CommandExecutionOptions
    {
        StandardInput = manifestYaml,
    },
    cancellationToken);
```

The text is written as UTF-8 without a byte order mark or an added newline. Include
any newline the tool requires in the string. Standard input closes after the text
is written; a null value closes it immediately.

The input text is never included in the command line. If a tool echoes sensitive
input, set `StandardInputIsSecret = true` to mask it in logs and failure diagnostics:

```csharp
await context.Shell.RunAsync(
    "docker",
    ["login", "registry.example.com", "--username", username, "--password-stdin"],
    new CommandExecutionOptions
    {
        StandardInput = registryPassword,
        StandardInputIsSecret = true,
    },
    cancellationToken);
```

`StandardInputIsSecret` defaults to `false`. Enabling it registers the whole input
and its encoded and per-line variants for the pipeline lifetime. Blank input is
ignored. Input shorter than `SecretMaskingOptions.MinimumSecretLength` is excluded
from framework masking and may remain visible in command diagnostics and logs;
native build-system masking is still requested when supported. Use it for
passwords or other small secret payloads. For large scripts or documents, register
only the sensitive values through `context.Security.Secrets.AddSecret(value)`
before execution instead; see [Secrets](secrets.md#register-secrets-discovered-at-runtime).
Successful `CommandResult.StandardOutput` and `StandardError` retain the tool's raw
output, so handle returned data containing secrets accordingly.

## Accepted Exit Codes

`CommandExecutionOptions.SuccessExitCodes` controls which exit codes count as
success. It defaults to `[0]`. For example, `git diff --exit-code` returns 1 when
there are differences:

```csharp
var result = await context.Shell.RunAsync(
    "git",
    ["diff", "--exit-code"],
    new CommandExecutionOptions
    {
        SuccessExitCodes = [0, 1],
    },
    cancellationToken);
```

A supplied collection replaces the default: include 0 explicitly to keep accepting
it. An empty collection accepts no exit codes; null is rejected.

Despite its name, `ThrowOnNonZeroExitCode` throws `CommandException` for any exit
code outside `SuccessExitCodes` when enabled (the default), including 0 if you
exclude it. Set it to `false` to return the result without an exit-code exception.
`SuccessExitCodes` still determines success in command logs and telemetry when
exceptions are disabled.

## Adding Unmodeled Options

Use `AdditionalArguments` when a strongly typed or generated options record does not yet
model a tool option. Each entry accepts a `CommandLinePhase`; entries with
`IsGlobalOption: true` appear before the command or subcommand parts.

```csharp
var options = new SomeGeneratedOptions
{
    AdditionalArguments =
    [
        new("--global-flag", IsGlobalOption: true),
        new("--new-option", CommandLinePhase.Normal),
        new("value", CommandLinePhase.Normal),
    ],
};
```

Within each non-terminal phase, additional tokens retain their declared order and appear
before generated tokens. The supported phases render as `EarlyOperand`, `Normal`,
`Passthrough`, `LateOperand`, then `Terminal`. Use `RunSettings` or a declared marker in `Arguments` for
end-of-options pass-through values. Terminal tokens appear after `Arguments` and cannot
be combined with an end-of-options marker or `RunSettings`.

## Strongly Typed Options

Static command identities use one source for each part:

- Put `[CliTool("tool-name")]` on the shared tool options base.
- Put `[CliSubCommand("first", "second")]` on command-specific options.
- Optionally use `[CliCommandAlias("short-form", IsPreferred = true)]` for the preferred subcommand alias.

For dynamic commands, set `Tool` and `CommandParts` at runtime. Non-null runtime values
override attributes; otherwise a preferred alias overrides `CliSubCommand`.

Mark a shared options base with `[CliGlobalOptions]` when its declared settings belong
before the subcommand. Global and command-local properties may use the same CLI switch
with different C# property names. Long names, short aliases, and negated names must remain
unique within each scope. `Terminal` properties render after the command regardless of
their declaring base, so they share the command-local scope for collision checks.

### Migrating from `CliCommandAttribute`

| Previous declaration | v4 declaration |
| --- | --- |
| `[CliCommand("npm", "token", "revoke")]` | `[CliTool("npm")]` on the base and `[CliSubCommand("token", "revoke")]` on the command |
| `[CliCommand("npx", "-c")]` on an npm-derived record | `[CliTool("npx")]` on that record and `[CliSubCommand("-c")]` |

`CliCommandAttribute` was removed in v4 because its tool slot overlapped with `CliToolAttribute`.
