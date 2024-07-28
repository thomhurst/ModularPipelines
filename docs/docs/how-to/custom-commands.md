---
title: Custom Commands
---

# Custom Commands
Many common CLI tools, such as npm, yarn, dotnet, docker, kubectl, have all had strong objects created to wrap around their CLI commands.

If you want to run a command that isn't currently supported by strong objects, you can still run commands directly through the `ICommand` interface available on the `context` object within your modules.

Every argument should be passed as a separate string in a collection. This allows proper formatting if there's things like spaces or quotes.

## Example

```csharp
await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("dotnet")
        {
            Arguments = new[] { "tool", "install", "--global", "dotnet-coverage" },
        }, cancellationToken);
```

This is the equivalent to running:

`dotnet tool install --global dotnet-coverage`