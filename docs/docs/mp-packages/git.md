---
title: Git Package
---

# Git Package

Git repository information, versioning, and strongly typed Git commands.

## Installation

```shell
dotnet add package ModularPipelines.Git
```

Required command-line tool: `git`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.Git.Extensions`, then use this service from a module:

- `context.Git()`

## Module example

```csharp
using ModularPipelines.Git.Extensions;

public class UseGitModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var git = context.Git();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Git integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.
