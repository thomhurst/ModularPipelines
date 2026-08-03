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
