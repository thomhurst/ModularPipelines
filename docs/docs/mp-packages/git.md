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

## Shared execution options

All command options inherit root execution settings from `GitOptions`. These settings
appear before the command name. For example:

```csharp
await context.Tools.Git.Commands.WorkingTree.StatusAsync(new GitStatusOptions
{
    ChangeDirectories = ["checkout", "application"],
    Configuration = [("core.quotepath", "false")],
    NoOptionalLocks = true,
    Short = true,
}, cancellationToken: cancellationToken);
```

`ChangeDirectories` repeats `-C` in collection order. Each relative directory resolves
against the previous directory; directory changes precede other root settings.
`Configuration` repeats `-c key=value` without collapsing duplicate keys. Use an empty
value to clear a setting or `"true"` for Git's implicit boolean form. Values use `KeyValue`
and are registered for secret masking, including HTTP authorization headers.
Secret masking does not remove values from Git's process arguments. Users able to
inspect those arguments may read credentials. Use `ConfigEnv` for credential-bearing
values that must not appear in Git's arguments.
`ConfigEnv` repeats `--config-env=name=ENVIRONMENT_VARIABLE`; it contains variable names,
not their secret contents.

`GitDirectory`, `WorkTree`, `Namespace`, and `ExecPath` use equals-separated values.
`ExecPath` requires a value; its bare path-reporting form is not an inherited setting.
Pager controls, replacement-object controls, optional-lock controls, pathspec settings,
`AttrSource`, `NoLazyFetch`, and `NoAdvice` are also shared. The installed Git version
must support each selected option. Command-local switches retain their separate scope:
`GitRevParseOptions.GitDir` reports a path, while `GitDirectory` selects the repository
before `rev-parse` executes. `BareRepository` sets root `--bare`; command-local `Bare`
properties such as `GitCloneOptions.Bare` still follow their command.

`GitBaseOptions` retains version and path-reporting actions for `Repository.GitAsync`.
These actions are not inherited by ordinary commands. Existing `GitBaseOptions.GitDir`
and `.Bare` initializers migrate to `.GitDirectory` and `.BareRepository` respectively;
other shared properties retain their names through inheritance.

The Git integration deliberately retains its handwritten grouped facade and option
ownership. Its scraper captures root usage separately from `git help -a`, validates the
complete command tree, and refreshes coverage metadata without replacing these records.
See the [Git command reference](https://git-scm.com/docs/git) for root-option semantics.

## Repository information

When the pipeline always runs inside a repository, use `GetRequiredInfoAsync`. It throws an
`InvalidOperationException` when Git information is unavailable, for example outside a repository or
when `git` is not installed:

```csharp
var repository = await context.Tools.Git.Information.GetRequiredInfoAsync(cancellationToken);
var branch = repository.BranchName;
var commit = repository.LastCommitSha;
```

Properties such as `BranchName` and `LastCommitSha` remain nullable, for example on a detached HEAD
or in a repository without commits. Use `GetInfoAsync` when the pipeline must handle running outside a
repository; it returns `null` instead of throwing.

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
combined with untracked, non-ignored files once per pipeline run. The comparison therefore includes
committed, staged, unstaged, and new files. A pattern without `*` or `?` matches both that exact path
and paths beneath it, so `src/MyService` can be used instead of `src/MyService/**`. If the base
revision is unavailable (for example, in a shallow checkout without `origin/main`), the condition
logs a warning and conservatively runs the module.

### Custom command runners

If you replace `IGitCommandRunner` and use changed-path checks, implement
`IRawGitCommandRunner` on the same class. `GitChanges` uses its untrimmed output to preserve
NUL-delimited Git paths exactly, including leading or trailing whitespace.

```csharp
builder.Services.AddSingleton<IGitCommandRunner, CustomGitCommandRunner>();
```

`CustomGitCommandRunner` must implement both interfaces on the same class.
