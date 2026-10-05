using System.Collections.Concurrent;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Caching;

internal sealed class ModuleCacheFileHasher
{
    private readonly int _maximumConcurrency;

    public ModuleCacheFileHasher(IOptions<ModuleCacheOptions> options)
    {
        _maximumConcurrency = Math.Max(1, options.Value.MaxHashConcurrency);
    }

    public async Task<IReadOnlyDictionary<string, string>> HashAsync(
        IReadOnlyList<string> paths,
        string workingDirectory,
        CancellationToken cancellationToken)
    {
        var hashes = new ConcurrentDictionary<string, string>(
            ModuleCacheFileResolver.GetPathComparer(workingDirectory));

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
        var stream = new FileStream(
            path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            64 * 1024,
            FileOptions.Asynchronous | FileOptions.SequentialScan);
        await using var streamLifetime = stream.ConfigureAwait(false);
        var hash = await SHA256.HashDataAsync(stream, cancellationToken).ConfigureAwait(false);
        return Convert.ToHexString(hash);
    }
}
