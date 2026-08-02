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

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Git`

## Module example

```csharp
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Git.Options;

public class UseGitModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Tools.Git.Commands.WorkingTree.StatusAsync(
            new GitStatusOptions
            {
                Short = true,
            },
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

## Run only when paths change

Use `RunIfChangedAttribute` to run a module when at least one repository-relative glob matches a
path changed since the merge base with `origin/main`:

```csharp
using ModularPipelines.Git.Attributes;

[RunIfChanged("src/MyService/**", "test/MyService.Tests/**")]
public class TestMyServiceModule : Module<CommandResult>
{
    // ...
}
```

Set another base revision with the named `Base` property:

```csharp
[RunIfChanged("src/**", Base = "origin/release")]
```

For imperative checks, use the same cached changed-path set through the Git context:

```csharp
var shouldBuild = await context.Tools.Git.Changes.HasChangesAsync(
    ["src/MyService/**", "Directory.Packages.props"],
    cancellationToken: cancellationToken);
```

Each base revision is resolved with `git merge-base`, and its `git diff --name-only` result is
computed once per pipeline run.
