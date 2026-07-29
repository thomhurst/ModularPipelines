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
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;

public class UseGitModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Git().Commands.WorkingTree.StatusAsync(
            new GitStatusOptions
            {
                Short = true,
            },
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.
