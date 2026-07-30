using System.Collections.Concurrent;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Caching;

internal sealed class ModuleCacheFileHasher
{
    private readonly int _maximumConcurrency;

    public ModuleCacheFileHasher(IOptions<ModuleCacheOptions> options)
    {
        _maximumConcurrency = Math.Max(1, options.Value.MaximumHashConcurrency);
    }

    public async Task<IReadOnlyDictionary<string, string>> HashAsync(
        IReadOnlyList<string> paths,
        CancellationToken cancellationToken)
    {
        var hashes = new ConcurrentDictionary<string, string>(GetPathComparer());

        await Parallel.ForEachAsync(
            paths,
            new ParallelOptions
            {
                MaxDegreeOfParallelism = _maximumConcurrency,
                CancellationToken = cancellationToken,
            },
            async (path, token) =>
            {
                var before = new FileInfo(path);
                var beforeLength = before.Length;
                var beforeLastWriteUtc = before.LastWriteTimeUtc;
                var hash = await HashFileAsync(path, token).ConfigureAwait(false);
                var after = new FileInfo(path);
                if (beforeLength != after.Length || beforeLastWriteUtc != after.LastWriteTimeUtc)
                {
                    hash = await HashFileAsync(path, token).ConfigureAwait(false);
                }

                hashes[path] = hash;
            }).ConfigureAwait(false);

        return hashes;
    }

    private static async Task<string> HashFileAsync(string path, CancellationToken cancellationToken)
    {
        await using var stream = new FileStream(
            path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            64 * 1024,
            FileOptions.Asynchronous | FileOptions.SequentialScan);
        var hash = await SHA256.HashDataAsync(stream, cancellationToken).ConfigureAwait(false);
        return Convert.ToHexString(hash);
    }

    private static StringComparer GetPathComparer() =>
        OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
}
