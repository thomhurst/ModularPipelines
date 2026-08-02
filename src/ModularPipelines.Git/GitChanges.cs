using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

internal sealed class GitChanges : IGitChanges, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ConcurrentDictionary<string, ChangeCacheEntry> _changesByBase =
        new(StringComparer.Ordinal);

    public GitChanges(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<bool> HasChangesAsync(
        IEnumerable<string> pathPatterns,
        string baseReference = "origin/main",
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(pathPatterns);
        ArgumentException.ThrowIfNullOrWhiteSpace(baseReference);

        var patterns = pathPatterns.Select(NormalizePattern).ToArray();
        if (patterns.Length == 0)
        {
            throw new ArgumentException("At least one path pattern is required.", nameof(pathPatterns));
        }

        var matcher = new Matcher(StringComparison.Ordinal);
        matcher.AddIncludePatterns(patterns);

        var snapshot = await GetChangedPathsAsync(baseReference, cancellationToken).ConfigureAwait(false);
        return !snapshot.IsKnown || snapshot.Paths.Any(path =>
            patterns.Contains(path, StringComparer.Ordinal)
            || matcher.Match(path).HasMatches);
    }

    public void Dispose()
    {
        foreach (var cacheEntry in _changesByBase.Values)
        {
            cacheEntry.Gate.Dispose();
        }
    }

    private async Task<ChangedPathSnapshot> GetChangedPathsAsync(
        string baseReference,
        CancellationToken cancellationToken)
    {
        var cacheEntry = _changesByBase.GetOrAdd(baseReference, static _ => new ChangeCacheEntry());
        await cacheEntry.Gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (cacheEntry.Snapshot is not null)
            {
                return cacheEntry.Snapshot;
            }

            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var gitCommandRunner = scope.ServiceProvider.GetRequiredService<IGitCommandRunner>();
            var mergeBase = await gitCommandRunner.RunCommandsOrNull(
                    null,
                    cancellationToken,
                    "merge-base",
                    baseReference,
                    "HEAD")
                .ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(mergeBase))
            {
                cacheEntry.Snapshot = ChangedPathSnapshot.Unknown;
                return cacheEntry.Snapshot;
            }

            var output = await RunCommandsUntrimmed(
                    gitCommandRunner,
                    new CommandExecutionOptions { MaxCapturedOutputLength = 0 },
                    cancellationToken,
                    "diff",
                    "--name-only",
                    "--no-renames",
                    "-z",
                    mergeBase,
                    "--")
                .ConfigureAwait(false);

            cacheEntry.Snapshot = new ChangedPathSnapshot(
                IsKnown: true,
                output.Split('\0', StringSplitOptions.RemoveEmptyEntries));
            return cacheEntry.Snapshot;
        }
        finally
        {
            cacheEntry.Gate.Release();
        }
    }

    private static string NormalizePattern(string pattern)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(pattern);
        var normalized = NormalizePatternSeparators(pattern).TrimStart('/');
        if (normalized == ".." || normalized.StartsWith("../", StringComparison.Ordinal))
        {
            throw new ArgumentException("Path patterns must be relative to the repository root.", nameof(pattern));
        }

        return normalized;
    }

    private static string NormalizePatternSeparators(string pattern) => pattern.Replace('\\', '/');

    private static Task<string> RunCommandsUntrimmed(
        IGitCommandRunner gitCommandRunner,
        CommandExecutionOptions? options,
        CancellationToken cancellationToken,
        params string?[] commands) =>
        gitCommandRunner is IRawGitCommandRunner rawGitCommandRunner
            ? rawGitCommandRunner.RunCommandsUntrimmed(options, cancellationToken, commands)
            : gitCommandRunner.RunCommands(options, cancellationToken, commands);

    private sealed class ChangeCacheEntry
    {
        public SemaphoreSlim Gate { get; } = new(1, 1);

        public ChangedPathSnapshot? Snapshot { get; set; }
    }

    private sealed record ChangedPathSnapshot(bool IsKnown, IReadOnlyList<string> Paths)
    {
        public static ChangedPathSnapshot Unknown { get; } = new(false, []);
    }
}
