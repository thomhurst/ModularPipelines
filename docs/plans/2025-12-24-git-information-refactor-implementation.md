# GitInformation Refactor Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Consolidate `StaticGitInformation` and `GitInformation` into a single class with cleaner architecture, non-nullable properties with sentinel values, and `Commits()` moved to `IGitCommands`.

**Architecture:** Single `GitInformation` class implements both `IGitInformation` and `IInitializer`. Registered as singleton, creates a scoped context during initialization to run git commands, then caches results. `Commits()` moves to `IGitCommands` for dynamic history traversal.

**Tech Stack:** C#, .NET, Microsoft.Extensions.DependencyInjection, IInitializer pattern from Initialization.Microsoft.Extensions.DependencyInjection

---

## Task 1: Update IGitInformation Interface

**Files:**
- Modify: `src/ModularPipelines.Git/IGitInformation.cs`

**Step 1: Update the interface to remove Commits() and make strings non-nullable**

Replace entire file content:

```csharp
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;

namespace ModularPipelines.Git;

public interface IGitInformation
{
    Folder Root { get; }
    string BranchName { get; }
    string DefaultBranchName { get; }
    string LastCommitSha { get; }
    string LastCommitShortSha { get; }
    string Tag { get; }
    int CommitsOnBranch { get; }
    DateTimeOffset LastCommitDateTime { get; }
    GitCommit? PreviousCommit { get; }
}
```

**Step 2: Build to verify compilation**

Run: `dotnet build src/ModularPipelines.Git/ModularPipelines.Git.csproj`
Expected: Build errors in `GitInformation.cs` (missing Commits methods) - this is expected

**Step 3: Commit**

```bash
git add src/ModularPipelines.Git/IGitInformation.cs
git commit -m "refactor(git): update IGitInformation interface - remove Commits, non-nullable strings"
```

---

## Task 2: Add Commits() to IGitCommands Interface

**Files:**
- Modify: `src/ModularPipelines.Git/IGitCommands.cs`

**Step 1: Add the Commits method signatures at the end of the interface**

Add before the closing brace of the interface:

```csharp
    IAsyncEnumerable<GitCommit> Commits(GitOptions? options = null, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GitCommit> Commits(string? branch, GitOptions? options = null, CancellationToken cancellationToken = default);
```

**Step 2: Add required using at top of file**

Add after existing usings:

```csharp
using System.Runtime.CompilerServices;
using ModularPipelines.Git.Models;
```

**Step 3: Build to verify interface compiles**

Run: `dotnet build src/ModularPipelines.Git/ModularPipelines.Git.csproj`
Expected: Build errors in `GitCommands.cs` (missing implementation) - this is expected

**Step 4: Commit**

```bash
git add src/ModularPipelines.Git/IGitCommands.cs
git commit -m "refactor(git): add Commits() methods to IGitCommands interface"
```

---

## Task 3: Implement Commits() in GitCommands

**Files:**
- Modify: `src/ModularPipelines.Git/GitCommands.cs`

**Step 1: Add required fields to GitCommands class**

Add after existing `_command` field (around line 11):

```csharp
    private readonly GitCommandRunner _gitCommandRunner;
    private readonly IGitCommitMapper _gitCommitMapper;
```

**Step 2: Update constructor to accept new dependencies**

Replace constructor (lines 13-16):

```csharp
    public GitCommands(ICommand command, GitCommandRunner gitCommandRunner, IGitCommitMapper gitCommitMapper)
    {
        _command = command;
        _gitCommandRunner = gitCommandRunner;
        _gitCommitMapper = gitCommitMapper;
    }
```

**Step 3: Add using statements at top of file**

Add after existing usings:

```csharp
using System.Runtime.CompilerServices;
using ModularPipelines.Git.Models;
```

**Step 4: Add Commits implementation at end of class before closing brace**

```csharp
    /// <inheritdoc/>
    public virtual IAsyncEnumerable<GitCommit> Commits(GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return Commits(null, options, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async IAsyncEnumerable<GitCommit> Commits(string? branch, GitOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var index = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            var output = await _gitCommandRunner.RunCommandsOrNull(options, "log", branch, $"--skip={index}", "-1", $"--format='%aN {GitConstants.GitEscapedLineSeparator} %aE {GitConstants.GitEscapedLineSeparator} %aI {GitConstants.GitEscapedLineSeparator} %cN {GitConstants.GitEscapedLineSeparator} %cE {GitConstants.GitEscapedLineSeparator} %cI {GitConstants.GitEscapedLineSeparator} %H {GitConstants.GitEscapedLineSeparator} %h {GitConstants.GitEscapedLineSeparator} %s {GitConstants.GitEscapedLineSeparator} %B'");

            if (string.IsNullOrWhiteSpace(output))
            {
                yield break;
            }

            index++;
            yield return _gitCommitMapper.Map(output);
        }
    }
```

**Step 5: Build to verify compilation**

Run: `dotnet build src/ModularPipelines.Git/ModularPipelines.Git.csproj`
Expected: PASS (may have warnings about GitInformation, that's OK)

**Step 6: Commit**

```bash
git add src/ModularPipelines.Git/GitCommands.cs
git commit -m "feat(git): implement Commits() methods in GitCommands"
```

---

## Task 4: Rewrite GitInformation as Consolidated Class

**Files:**
- Modify: `src/ModularPipelines.Git/GitInformation.cs`

**Step 1: Replace entire GitInformation.cs with consolidated implementation**

```csharp
using System.Runtime.CompilerServices;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

internal class GitInformation : IGitInformation, IInitializer
{
    private readonly IServiceProvider _serviceProvider;

    public GitInformation(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

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
        await using var scope = _serviceProvider.CreateAsyncScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<GitInformation>>();
        var command = scope.ServiceProvider.GetRequiredService<ICommand>();
        var gitCommandRunner = scope.ServiceProvider.GetRequiredService<GitCommandRunner>();
        var gitCommitMapper = scope.ServiceProvider.GetRequiredService<IGitCommitMapper>();

        await VerifyGitAvailable(command);

        var tasks = new List<Task>();

        Async(async () =>
        {
            var root = await GetOutput(command, logger, new GitRevParseOptions { ShowToplevel = true });
            Root = root != null ? new Folder(root) : throw new InvalidOperationException("Not a git repository");
        });

        Async(async () => BranchName = await GetOutput(command, logger, new GitBranchOptions { ShowCurrent = true }) ?? "");

        Async(async () => DefaultBranchName = await GetDefaultBranchName(command, logger) ?? "");

        Async(async () => LastCommitSha = await GetOutput(command, logger, new GitRevParseOptions { Arguments = ["HEAD"] }) ?? "");

        Async(async () => LastCommitShortSha = await GetOutput(command, logger, new GitRevParseOptions { Short = true, Arguments = ["HEAD"] }) ?? "");

        Async(async () => Tag = await GetOutput(command, logger, new GitDescribeOptions { Tags = true }) ?? "");

        Async(async () =>
        {
            var countStr = await GetOutput(command, logger, new GitRevListOptions { Count = true, Arguments = ["HEAD"] });
            CommitsOnBranch = int.TryParse(countStr, out var count) ? count : 0;
        });

        Async(async () =>
        {
            var timestampStr = await GetOutput(command, logger, new GitLogOptions { Format = "%at", Arguments = ["-1"] });
            LastCommitDateTime = long.TryParse(timestampStr, out var timestamp)
                ? DateTimeOffset.FromUnixTimeSeconds(timestamp)
                : DateTimeOffset.MinValue;
        });

        Async(async () => PreviousCommit = await GetPreviousCommit(gitCommandRunner, gitCommitMapper));

        await Task.WhenAll(tasks);
        return;

        void Async(Func<Task> task) => tasks.Add(task());
    }

    private static async Task VerifyGitAvailable(ICommand command)
    {
        try
        {
            await command.ExecuteCommandLineTool(new CommandLineToolOptions("git")
            {
                Arguments = ["version"],
                LogSettings = CommandLoggingOptions.Silent,
            });
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("Git is not available. Ensure git is installed and in PATH.", e);
        }
    }

    private static async Task<string?> GetDefaultBranchName(ICommand command, ILogger logger)
    {
        try
        {
            var output = await GetOutput(command, logger, new GitRemoteOptions { Arguments = ["show", "origin"] });
            if (output == null)
            {
                return null;
            }

            return output.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(x => x.StartsWith("HEAD branch:"))
                ?.Split("HEAD branch: ")[1];
        }
        catch
        {
            var output = await GetOutput(command, logger, new GitRevParseOptions
            {
                Arguments = ["origin/HEAD"],
                AbbrevRef = true,
                ThrowOnNonZeroExitCode = false,
            });

            return output?.Replace("origin/", string.Empty);
        }
    }

    private static async Task<string?> GetOutput(ICommand command, ILogger logger, GitOptions gitOptions)
    {
        try
        {
            var result = await command.ExecuteCommandLineTool(gitOptions with
            {
                LogSettings = logger.IsEnabled(LogLevel.Debug) ? CommandLoggingOptions.Diagnostic : CommandLoggingOptions.Silent,
            });
            return result.StandardOutput.Trim();
        }
        catch (Exception exception)
        {
            logger.LogDebug(exception, "Error running Git command");
            return null;
        }
    }

    private static async Task<GitCommit?> GetPreviousCommit(GitCommandRunner gitCommandRunner, IGitCommitMapper gitCommitMapper)
    {
        var output = await gitCommandRunner.RunCommandsOrNull(null, "log", "--skip=0", "-1",
            $"--format='%aN {GitConstants.GitEscapedLineSeparator} %aE {GitConstants.GitEscapedLineSeparator} %aI {GitConstants.GitEscapedLineSeparator} %cN {GitConstants.GitEscapedLineSeparator} %cE {GitConstants.GitEscapedLineSeparator} %cI {GitConstants.GitEscapedLineSeparator} %H {GitConstants.GitEscapedLineSeparator} %h {GitConstants.GitEscapedLineSeparator} %s {GitConstants.GitEscapedLineSeparator} %b'");

        return string.IsNullOrWhiteSpace(output) ? null : gitCommitMapper.Map(output);
    }
}
```

**Step 2: Build to verify compilation**

Run: `dotnet build src/ModularPipelines.Git/ModularPipelines.Git.csproj`
Expected: PASS

**Step 3: Commit**

```bash
git add src/ModularPipelines.Git/GitInformation.cs
git commit -m "refactor(git): consolidate GitInformation with IInitializer and sentinel values"
```

---

## Task 5: Delete StaticGitInformation

**Files:**
- Delete: `src/ModularPipelines.Git/StaticGitInformation.cs`

**Step 1: Delete the file**

```bash
git rm src/ModularPipelines.Git/StaticGitInformation.cs
```

**Step 2: Build to check for any remaining references**

Run: `dotnet build src/ModularPipelines.Git/ModularPipelines.Git.csproj`
Expected: Error in GitExtensions.cs referencing StaticGitInformation - this is expected

**Step 3: Commit**

```bash
git commit -m "refactor(git): delete StaticGitInformation (merged into GitInformation)"
```

---

## Task 6: Update DI Registration

**Files:**
- Modify: `src/ModularPipelines.Git/Extensions/GitExtensions.cs`

**Step 1: Update RegisterGitContext method**

Replace the `RegisterGitContext` method (lines 21-31):

```csharp
    public static IServiceCollection RegisterGitContext(this IServiceCollection services)
    {
        services.TryAddScoped<IGit, Git>();
        services.TryAddScoped<IGitCommands, GitCommands>();
        services.TryAddSingleton<IGitInformation, GitInformation>();
        services.TryAddScoped<IGitVersioning, GitVersioning>();
        services.TryAddScoped<GitCommandRunner>();
        services.TryAddSingleton<IGitCommitMapper, GitCommitMapper>();
        return services;
    }
```

**Step 2: Build to verify compilation**

Run: `dotnet build src/ModularPipelines.Git/ModularPipelines.Git.csproj`
Expected: PASS

**Step 3: Commit**

```bash
git add src/ModularPipelines.Git/Extensions/GitExtensions.cs
git commit -m "refactor(git): update DI registration - GitInformation as singleton"
```

---

## Task 7: Build Full Solution

**Step 1: Build entire solution**

Run: `dotnet build ModularPipelines.sln -c Release`
Expected: PASS (or identify any other files needing updates)

**Step 2: If errors, fix them before proceeding**

Any errors should be related to callers of `IGitInformation.Commits()` - they need to use `IGitCommands.Commits()` instead.

**Step 3: Commit any additional fixes**

```bash
git add -A
git commit -m "fix(git): update any remaining references after refactor"
```

---

## Task 8: Update Unit Tests

**Files:**
- Modify: `test/ModularPipelines.UnitTests/GitInformationTests.cs`

**Step 1: Verify existing test still compiles and works**

Run: `dotnet build test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj`
Expected: PASS

**Step 2: Run the git information test**

Run: `dotnet test test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj --filter "FullyQualifiedName~GitInformationTests"`
Expected: PASS

**Step 3: Commit if any changes were needed**

```bash
git add test/ModularPipelines.UnitTests/GitInformationTests.cs
git commit -m "test(git): update GitInformation tests for refactored API"
```

---

## Task 9: Run Full Test Suite

**Step 1: Run all unit tests**

Run: `dotnet test test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj`
Expected: All tests PASS

**Step 2: If any failures, investigate and fix**

Common issues:
- Tests depending on `Commits()` being on `IGitInformation`
- Tests mocking `StaticGitInformation` directly

**Step 3: Commit any test fixes**

```bash
git add -A
git commit -m "test: fix tests after GitInformation refactor"
```

---

## Task 10: Final Verification and Cleanup

**Step 1: Run full solution build**

Run: `dotnet build ModularPipelines.sln -c Release`
Expected: PASS with no warnings related to our changes

**Step 2: Verify no TODO comments or incomplete code**

Run: `grep -r "TODO" src/ModularPipelines.Git/`
Expected: No TODOs related to this refactor

**Step 3: Create summary commit if needed**

```bash
git log --oneline -10
```

Review commits to ensure clean history.

---

## Migration Notes for Users

Add to release notes:

```markdown
## Breaking Changes

### IGitInformation.Commits() Moved to IGitCommands

The `Commits()` methods have moved from `IGitInformation` to `IGitCommands`:

```csharp
// Before
context.Git().Information.Commits()

// After
context.Git().Commands.Commits()
```

### String Properties Now Non-Nullable

Properties like `BranchName`, `Tag`, `LastCommitSha` are now non-nullable strings.
They return empty string `""` when the value is not available (e.g., no tag exists).
```
