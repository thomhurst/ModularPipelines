# Git Package

Git repository information, versioning, and strongly typed Git commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Git
```

Required command-line tool: `git`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.Git.Extensions`, then use this service from a module:

* `context.Git()`

## Module example[​](#module-example "Direct link to Module example")

```
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

The package exposes generated options records for its supported CLI commands.
