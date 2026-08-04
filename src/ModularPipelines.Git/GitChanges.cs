using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

internal sealed class GitChanges : IGitChanges, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<GitChanges> _logger;
    private readonly ConcurrentDictionary<string, ChangeCacheEntry> _changesByBase =
        new(StringComparer.Ordinal);

    public GitChanges(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<GitChanges>? logger = null)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger ?? NullLogger<GitChanges>.Instance;
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
        return !snapshot.IsKnown || snapshot.Paths.Any(path => Matches(path, patterns, matcher));
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
                _logger.LogWarning(
                    "Could not resolve Git merge base '{BaseReference}' against HEAD. "
                    + "RunIfChanged will conservatively report changes.",
                    baseReference);
                cacheEntry.Snapshot = ChangedPathSnapshot.Unknown;
                return cacheEntry.Snapshot;
            }

            var executionOptions = new CommandExecutionOptions { MaxCapturedOutputLength = 0 };
            var diffOutput = await RunCommandsUntrimmed(
                    gitCommandRunner,
                    executionOptions,
                    cancellationToken,
                    "diff",
                    "--name-only",
                    "--no-renames",
                    "-z",
                    mergeBase,
                    "--")
                .ConfigureAwait(false);
            var untrackedOutput = await RunCommandsUntrimmed(
                    gitCommandRunner,
                    executionOptions,
                    cancellationToken,
                    "ls-files",
                    "--others",
                    "--exclude-standard",
                    "-z")
                .ConfigureAwait(false);

            cacheEntry.Snapshot = new ChangedPathSnapshot(
                IsKnown: true,
                SplitPaths(diffOutput)
                    .Concat(SplitPaths(untrackedOutput))
                    .Distinct(StringComparer.Ordinal)
                    .ToArray());
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

    private static bool Matches(string path, IReadOnlyList<string> patterns, Matcher matcher)
    {
        if (matcher.Match(path).HasMatches)
        {
            return true;
        }

        return patterns
            .Where(pattern => pattern.IndexOfAny(['*', '?']) < 0)
            .Any(pattern => path.Equals(pattern, StringComparison.Ordinal)
                            || path.StartsWith(pattern.TrimEnd('/') + '/', StringComparison.Ordinal));
    }

    private static IEnumerable<string> SplitPaths(string output) =>
        output.Split('\0', StringSplitOptions.RemoveEmptyEntries);

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
