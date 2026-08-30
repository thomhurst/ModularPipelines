# Git Package

Git repository information, versioning, and strongly typed Git commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Git
```

Required command-line tool: `git`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Git`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines;

using ModularPipelines.Git.Options;



public class UseGitModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

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

## Run only when paths change[​](#run-only-when-paths-change "Direct link to Run only when paths change")

Use `RunIfChangedAttribute` to run a module when at least one repository-relative glob matches a path changed since the merge base with `origin/main`:

```
using ModularPipelines.Git.Attributes;



[RunIfChanged("src/MyService/**", "test/MyService.Tests/**")]

public class TestMyServiceModule : Module<CommandResult>

{

    // ...

}
```

Set another base revision with the named `Base` property:

```
[RunIfChanged("src/**", Base = "origin/release")]
```

For imperative checks, use the same cached changed-path set through the Git context:

```
var shouldBuild = await context.Tools.Git.Changes.HasChangesAsync(

    ["src/MyService/**", "Directory.Packages.props"],

    cancellationToken: cancellationToken);
```

Each base revision is resolved with `git merge-base`, and its `git diff --name-only` result is combined with untracked, non-ignored files once per pipeline run. The comparison therefore includes committed, staged, unstaged, and new files. A pattern without `*` or `?` matches both that exact path and paths beneath it, so `src/MyService` can be used instead of `src/MyService/**`. If the base revision is unavailable (for example, in a shallow checkout without `origin/main`), the condition logs a warning and conservatively runs the module.

### Custom command runners[​](#custom-command-runners "Direct link to Custom command runners")

If you replace `IGitCommandRunner` and use changed-path checks, implement `IRawGitCommandRunner` on the same class. `GitChanges` uses its untrimmed output to preserve NUL-delimited Git paths exactly, including leading or trailing whitespace.

```
builder.Services.AddSingleton<IGitCommandRunner, CustomGitCommandRunner>();
```

`CustomGitCommandRunner` must implement both interfaces on the same class.
