# GitInformation Refactor Design

**Issue:** [#1412](https://github.com/thomhurst/ModularPipelines/issues/1412)
**Date:** 2025-12-24
**Status:** Approved

## Problem

The current `GitInformation` and `StaticGitInformation` architecture has several issues:

1. **Misleading naming** - `StaticGitInformation` is a cached singleton, not truly static
2. **Temporal coupling** - Properties nullable until `InitializeAsync()` completes
3. **Redundant wrapper** - `GitInformation` delegates to `StaticGitInformation` with little added value
4. **Split responsibilities** - Unclear separation between the two classes
5. **Anti-pattern DI** - `StaticGitInformation` uses `IServiceProvider` to resolve dependencies

## Design Decisions

| Decision | Choice |
|----------|--------|
| Primary goal | Simplify API, improve initialization safety, better testability |
| Nullable handling | Sentinel values (empty strings) instead of null |
| Initialization | Keep `IInitializer` pattern |
| Class structure | Single merged `GitInformation` class |
| `Commits()` location | Move to `IGitCommands` |
| Testing approach | Interface mocking is sufficient |
| Breaking changes | Clean break (major version bump) |
| `PreviousCommit` | Keep nullable (semantic null for "no previous commit") |

## New Interface

```csharp
public interface IGitInformation
{
    Folder Root { get; }
    string BranchName { get; }           // "" if detached HEAD
    string DefaultBranchName { get; }    // "" if cannot be determined
    string LastCommitSha { get; }        // "" if no commits
    string LastCommitShortSha { get; }   // "" if no commits
    string Tag { get; }                  // "" if no tag
    int CommitsOnBranch { get; }         // 0 if no commits
    DateTimeOffset LastCommitDateTime { get; }  // MinValue if no commits
    GitCommit? PreviousCommit { get; }   // null if no previous commit
}
```

**Breaking changes:**
- Removed `Commits()` methods (moved to `IGitCommands`)
- String properties now non-nullable with sentinel values

## Implementation

### Consolidated GitInformation Class

```csharp
internal class GitInformation : IGitInformation, IInitializer
{
    private readonly ILogger<GitInformation> _logger;
    private readonly ICommand _command;
    private readonly GitCommandRunner _gitCommandRunner;
    private readonly IGitCommitMapper _gitCommitMapper;

    public GitInformation(
        ILogger<GitInformation> logger,
        ICommand command,
        GitCommandRunner gitCommandRunner,
        IGitCommitMapper gitCommitMapper)
    {
        _logger = logger;
        _command = command;
        _gitCommandRunner = gitCommandRunner;
        _gitCommitMapper = gitCommitMapper;
    }

    // Properties with sentinel defaults
    public Folder Root { get; private set; } = null!;
    public string BranchName { get; private set; } = "";
    public string DefaultBranchName { get; private set; } = "";
    public string LastCommitSha { get; private set; } = "";
    public string LastCommitShortSha { get; private set; } = "";
    public string Tag { get; private set; } = "";
    public int CommitsOnBranch { get; private set; }
    public DateTimeOffset LastCommitDateTime { get; private set; }
    public GitCommit? PreviousCommit { get; private set; }

    public async Task InitializeAsync()
    {
        // Throws if git not available or not a repository
        await VerifyGitAvailable();
        Root = await GetRepositoryRoot();

        // Parallel fetch with sentinel fallbacks
        // ... (same logic as current StaticGitInformation.InitializeAsync)
    }
}
```

### Commits() on IGitCommands

```csharp
public interface IGitCommands
{
    // ... existing methods ...

    IAsyncEnumerable<GitCommit> Commits(
        GitOptions? options = null,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GitCommit> Commits(
        string branch,
        GitOptions? options = null,
        CancellationToken cancellationToken = default);
}
```

### DI Registration

```csharp
internal static IServiceCollection AddGit(this IServiceCollection services)
{
    services.TryAddScoped<IGit, Git>();
    services.TryAddScoped<IGitCommands, GitCommands>();
    services.TryAddSingleton<IGitInformation, GitInformation>();  // singleton
    services.TryAddScoped<IGitVersioning, GitVersioning>();
    services.TryAddSingleton<GitCommandRunner>();  // singleton (was scoped)
    services.TryAddSingleton<IGitCommitMapper, GitCommitMapper>();
    return services;
}
```

## Error Handling

| Scenario | Behavior |
|----------|----------|
| Not a git repository | Throw - pipeline cannot proceed |
| `git version` fails | Throw - git not installed |
| Branch unavailable (detached HEAD) | `BranchName = ""` |
| Tag unavailable | `Tag = ""` |
| Remote unavailable | `DefaultBranchName = ""` |

## Files Changed

**Delete:**
- `src/ModularPipelines.Git/StaticGitInformation.cs`

**Modify:**
- `src/ModularPipelines.Git/IGitInformation.cs`
- `src/ModularPipelines.Git/GitInformation.cs`
- `src/ModularPipelines.Git/IGitCommands.cs`
- `src/ModularPipelines.Git/GitCommands.cs`
- `src/ModularPipelines.Git/Extensions/GitExtensions.cs`
- `test/ModularPipelines.UnitTests/GitInformationTests.cs`

## Migration Guide

```csharp
// Commits() moved from Information to Commands
// Before:
context.Git().Information.Commits()

// After:
context.Git().Commands.Commits()
```
