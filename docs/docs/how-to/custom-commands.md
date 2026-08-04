---
title: Custom Commands
---

# Custom Commands
Many common CLI tools, such as npm, yarn, dotnet, docker, kubectl, have all had strong objects created to wrap around their CLI commands.

If you want to run a command that isn't currently supported by strong objects, you can still run commands directly through the `ICommandContext` interface available via `context.Shell.Command` within your modules.

Every argument should be passed as a separate string in a collection. This allows proper formatting if there's things like spaces or quotes.

## Example

```csharp
await context.Shell.Command.ExecuteCommandLineToolAsync(new GenericCommandLineToolOptions("dotnet")
        {
            Arguments = new[] { "tool", "install", "--global", "dotnet-coverage" },
        }, cancellationToken);
```

This is the equivalent to running:

`dotnet tool install --global dotnet-coverage`

By default, `Arguments` appears after generated non-terminal options and operands. It
appears before `RunSettings` (and its `--` marker) and before options in the `Terminal`
phase. When `ArgumentsContainToolOptions` is enabled, recognized tool options can be
hoisted ahead of a structured or declared end-of-options marker.

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
`Passthrough`, then `Terminal`. Use `RunSettings` or a declared marker in `Arguments` for
end-of-options pass-through values. Terminal tokens appear after `Arguments` and cannot
be combined with an end-of-options marker or `RunSettings`.

## Strongly Typed Options

Static command identities use one source for each part:

- Put `[CliTool("tool-name")]` on the shared tool options base.
- Put `[CliSubCommand("first", "second")]` on command-specific options.
- Optionally use `[CliCommandAlias("short-form", IsPreferred = true)]` for the preferred subcommand alias.

For dynamic commands, set `Tool` and `CommandParts` at runtime. Non-null runtime values
override attributes; otherwise a preferred alias overrides `CliSubCommand`.

### Migrating from `CliCommandAttribute`

| Previous declaration | v4 declaration |
| --- | --- |
| `[CliCommand("npm", "token", "revoke")]` | `[CliTool("npm")]` on the base and `[CliSubCommand("token", "revoke")]` on the command |
| `[CliCommand("npx", "-c")]` on an npm-derived record | `[CliTool("npx")]` on that record and `[CliSubCommand("-c")]` |

`CliCommandAttribute` was removed in v4 because its tool slot overlapped with `CliToolAttribute`.
