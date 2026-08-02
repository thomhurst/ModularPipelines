using System.Collections.Concurrent;
using Microsoft.Extensions.FileSystemGlobbing;

namespace ModularPipelines.Git;

internal sealed class GitChanges : IGitChanges, IDisposable
{
    private readonly IGitCommandRunner _gitCommandRunner;
    private readonly ConcurrentDictionary<string, ChangeCacheEntry> _changesByBase =
        new(StringComparer.Ordinal);

    public GitChanges(IGitCommandRunner gitCommandRunner)
    {
        _gitCommandRunner = gitCommandRunner;
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

        var changedPaths = await GetChangedPathsAsync(baseReference, cancellationToken).ConfigureAwait(false);
        return changedPaths.Any(path => matcher.Match(path).HasMatches);
    }

    public void Dispose()
    {
        foreach (var cacheEntry in _changesByBase.Values)
        {
            cacheEntry.Gate.Dispose();
        }
    }

    private async Task<IReadOnlyList<string>> GetChangedPathsAsync(
        string baseReference,
        CancellationToken cancellationToken)
    {
        var cacheEntry = _changesByBase.GetOrAdd(baseReference, static _ => new ChangeCacheEntry());
        await cacheEntry.Gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (cacheEntry.Paths is not null)
            {
                return cacheEntry.Paths;
            }

            var mergeBase = await _gitCommandRunner.RunCommands(
                    null,
                    cancellationToken,
                    "merge-base",
                    baseReference,
                    "HEAD")
                .ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(mergeBase))
            {
                throw new InvalidOperationException(
                    $"Git did not return a merge base for '{baseReference}' and HEAD.");
            }

            var output = await _gitCommandRunner.RunCommands(
                    null,
                    cancellationToken,
                    "diff",
                    "--name-only",
                    "-z",
                    mergeBase,
                    "HEAD",
                    "--")
                .ConfigureAwait(false);

            cacheEntry.Paths = output
                .Split('\0', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(NormalizePath)
                .ToArray();
            return cacheEntry.Paths;
        }
        finally
        {
            cacheEntry.Gate.Release();
        }
    }

    private static string NormalizePattern(string pattern)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(pattern);
        var normalized = NormalizePath(pattern.Trim()).TrimStart('/');
        if (normalized == ".." || normalized.StartsWith("../", StringComparison.Ordinal))
        {
            throw new ArgumentException("Path patterns must be relative to the repository root.", nameof(pattern));
        }

        return normalized;
    }

    private static string NormalizePath(string path) => path.Replace('\\', '/');

    private sealed class ChangeCacheEntry
    {
        public SemaphoreSlim Gate { get; } = new(1, 1);

        public IReadOnlyList<string>? Paths { get; set; }
    }
}
