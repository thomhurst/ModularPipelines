using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Caching;

internal sealed class ModuleCacheFileHasher
{
    private readonly ConcurrentDictionary<string, ModuleCacheFileHashRecord> _records;
    private readonly string _indexPath;
    private readonly int _maximumConcurrency;
    private readonly SemaphoreSlim _saveLock = new(1, 1);

    public ModuleCacheFileHasher(IOptions<ModuleCacheOptions> options)
    {
        _maximumConcurrency = Math.Max(1, options.Value.MaximumHashConcurrency);
        _indexPath = Path.Combine(Path.GetFullPath(options.Value.CacheDirectory), "file-hashes.json");
        _records = LoadIndex(_indexPath);
    }

    public async Task<IReadOnlyDictionary<string, string>> HashAsync(
        IReadOnlyList<string> paths,
        CancellationToken cancellationToken)
    {
        var hashes = new ConcurrentDictionary<string, string>(GetPathComparer());
        var changed = 0;

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
                if (_records.TryGetValue(path, out var record)
                    && record.Length == beforeLength
                    && record.LastWriteUtcTicks == beforeLastWriteUtc.Ticks)
                {
                    hashes[path] = record.Sha256;
                    return;
                }

                var hash = await HashFileAsync(path, token).ConfigureAwait(false);
                var after = new FileInfo(path);
                if (beforeLength != after.Length || beforeLastWriteUtc != after.LastWriteTimeUtc)
                {
                    hash = await HashFileAsync(path, token).ConfigureAwait(false);
                    after.Refresh();
                }

                hashes[path] = hash;
                _records[path] = new ModuleCacheFileHashRecord(after.Length, after.LastWriteTimeUtc.Ticks, hash);
                Interlocked.Exchange(ref changed, 1);
            }).ConfigureAwait(false);

        if (changed != 0)
        {
            await SaveIndexAsync(cancellationToken).ConfigureAwait(false);
        }

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

    private async Task SaveIndexAsync(CancellationToken cancellationToken)
    {
        await _saveLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var directory = Path.GetDirectoryName(_indexPath)!;
            Directory.CreateDirectory(directory);
            var temporary = $"{_indexPath}.{Guid.NewGuid():N}.tmp";
            try
            {
                await using (var stream = new FileStream(
                                 temporary,
                                 FileMode.CreateNew,
                                 FileAccess.Write,
                                 FileShare.None,
                                 16 * 1024,
                                 FileOptions.Asynchronous))
                {
                    var snapshot = _records.ToDictionary(
                        pair => pair.Key,
                        pair => pair.Value,
                        GetPathComparer());
                    await JsonSerializer.SerializeAsync(
                            stream,
                            snapshot,
                            ModuleCacheJsonSerializerContext.Default.DictionaryStringModuleCacheFileHashRecord,
                            cancellationToken)
                        .ConfigureAwait(false);
                }

                File.Move(temporary, _indexPath, overwrite: true);
            }
            finally
            {
                File.Delete(temporary);
            }
        }
        finally
        {
            _saveLock.Release();
        }
    }

    private static ConcurrentDictionary<string, ModuleCacheFileHashRecord> LoadIndex(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                var records = JsonSerializer.Deserialize(
                    File.ReadAllText(path),
                    ModuleCacheJsonSerializerContext.Default.DictionaryStringModuleCacheFileHashRecord);
                if (records is not null)
                {
                    return new ConcurrentDictionary<string, ModuleCacheFileHashRecord>(records, GetPathComparer());
                }
            }
        }
        catch (JsonException)
        {
            // A corrupt optimization index only causes files to be re-hashed.
        }
        catch (IOException)
        {
            // A concurrently replaced optimization index only causes files to be re-hashed.
        }

        return new ConcurrentDictionary<string, ModuleCacheFileHashRecord>(GetPathComparer());
    }

    private static StringComparer GetPathComparer() =>
        OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
}

internal sealed record ModuleCacheFileHashRecord(long Length, long LastWriteUtcTicks, string Sha256);
