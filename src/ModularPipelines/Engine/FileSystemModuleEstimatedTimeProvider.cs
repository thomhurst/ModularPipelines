using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Reporting;

namespace ModularPipelines.Engine;

/// <summary>
/// Default estimated-time provider. Stores the latest measured duration of each module and
/// sub-module as a text file under the user's application-data folder.
/// </summary>
/// <remarks>
/// Storage is best-effort: unreadable or malformed entries fall back to <see cref="DefaultEstimate"/>,
/// and writes go to a unique temporary file that atomically replaces the entry, so concurrent
/// pipelines never observe partially written values.
/// </remarks>
internal class FileSystemModuleEstimatedTimeProvider : IModuleEstimatedTimeProvider
{
    internal static readonly TimeSpan DefaultEstimate = TimeSpan.FromMinutes(2);

    private static readonly TimeSpan CacheRetention = TimeSpan.FromDays(90);
    private static readonly TimeSpan IndexRefreshInterval = TimeSpan.FromMinutes(1);

    private readonly Lock _subModuleIndexLock = new();
    private readonly string _directory;
    private readonly TimeProvider _timeProvider;
    private SubModuleFileIndex? _subModuleFileIndex;

    public FileSystemModuleEstimatedTimeProvider()
        : this(
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ModularPipelines",
                "EstimatedTimes"),
            TimeProvider.System)
    {
    }

    internal FileSystemModuleEstimatedTimeProvider(string directory, TimeProvider timeProvider)
    {
        _directory = directory;
        _timeProvider = timeProvider;
    }

    public async Task<TimeSpan> GetModuleEstimatedTimeAsync(ModuleId moduleId, CancellationToken cancellationToken = default)
    {
        var fileName = $"{GetStorageKey(moduleId)}.txt";
        var entry = await ReadEntryAsync(fileName, cancellationToken).ConfigureAwait(false);
        return entry?.Duration ?? DefaultEstimate;
    }

    public async Task SaveModuleTimeAsync(ModuleId moduleId, TimeSpan duration, CancellationToken cancellationToken = default)
    {
        var fileName = $"{GetStorageKey(moduleId)}.txt";

        var entry = new EstimatedTimeEntry(moduleId.Value, null, duration);
        await SaveEntryAsync(entry, fileName, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(
        ModuleId moduleId,
        CancellationToken cancellationToken = default)
    {
        var moduleName = GetStorageKey(moduleId);
        var filesByModule = GetSubModuleFilesByModule();
        var paths = filesByModule.GetValueOrDefault(moduleName, []);

        var subModuleEstimations = await paths.ToAsyncProcessorBuilder()
            .SelectAsync(file => ReadSubModuleEstimationAsync(file, cancellationToken))
            .ProcessInParallel()
            .GetResultsAsync()
            .ConfigureAwait(false);

        return subModuleEstimations.OfType<SubModuleEstimation>();
    }

    public async Task SaveSubModuleTimeAsync(
        ModuleId moduleId,
        SubModuleEstimation subModuleEstimation,
        CancellationToken cancellationToken = default)
    {
        var moduleName = GetStorageKey(moduleId);
        var subModuleHash = GetHash(subModuleEstimation.SubModuleName);
        var fileName = $"Mod-{moduleName}-Sub-{subModuleHash}.txt";

        var entry = new EstimatedTimeEntry(
            moduleId.Value,
            subModuleEstimation.SubModuleName,
            subModuleEstimation.EstimatedDuration);
        await SaveEntryAsync(entry, fileName, cancellationToken).ConfigureAwait(false);

        lock (_subModuleIndexLock)
        {
            Volatile.Write(ref _subModuleFileIndex, null);
        }
    }

    private async Task<SubModuleEstimation?> ReadSubModuleEstimationAsync(
        FileInfo file,
        CancellationToken cancellationToken)
    {
        var entry = await ReadEntryAsync(file.FullName, cancellationToken).ConfigureAwait(false);
        return entry?.SubModuleName is { } subModuleName
            ? new SubModuleEstimation(subModuleName, entry.Duration)
            : null;
    }

    private IReadOnlyDictionary<string, FileInfo[]> GetSubModuleFilesByModule()
    {
        var index = Volatile.Read(ref _subModuleFileIndex);
        if (index is not null
            && _timeProvider.GetElapsedTime(index.CreatedTimestamp) < IndexRefreshInterval)
        {
            return index.FilesByModule;
        }

        lock (_subModuleIndexLock)
        {
            index = _subModuleFileIndex;
            if (index is not null
                && _timeProvider.GetElapsedTime(index.CreatedTimestamp) < IndexRefreshInterval)
            {
                return index.FilesByModule;
            }

            index = new SubModuleFileIndex(
                BuildSubModuleFileIndex(_timeProvider.GetUtcNow()),
                _timeProvider.GetTimestamp());
            Volatile.Write(ref _subModuleFileIndex, index);
            return index.FilesByModule;
        }
    }

    private Dictionary<string, FileInfo[]> BuildSubModuleFileIndex(DateTimeOffset now)
    {
        var directoryInfo = Directory.CreateDirectory(_directory);
        var expirationTime = now.UtcDateTime - CacheRetention;
        var filesByModule = new Dictionary<string, List<FileInfo>>(StringComparer.Ordinal);

        foreach (var file in directoryInfo.EnumerateFiles("*.txt", SearchOption.TopDirectoryOnly))
        {
            if (file.LastWriteTimeUtc < expirationTime)
            {
                TryDelete(file);
                continue;
            }

            if (!TryGetModuleName(file.Name, out var moduleName))
            {
                continue;
            }

            if (!filesByModule.TryGetValue(moduleName, out var files))
            {
                files = [];
                filesByModule[moduleName] = files;
            }

            files.Add(file);
        }

        return filesByModule.ToDictionary(
            pair => pair.Key,
            pair => pair.Value.ToArray(),
            StringComparer.Ordinal);
    }

    private static bool TryGetModuleName(string fileName, out string moduleName)
    {
        const string prefix = "Mod-";
        const string separator = "-Sub-";

        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var subModuleSeparatorIndex = fileNameWithoutExtension.IndexOf(separator, StringComparison.Ordinal);
        if (!fileNameWithoutExtension.StartsWith(prefix, StringComparison.Ordinal)
            || subModuleSeparatorIndex <= prefix.Length)
        {
            moduleName = string.Empty;
            return false;
        }

        moduleName = fileNameWithoutExtension[prefix.Length..subModuleSeparatorIndex];
        return true;
    }

    internal static string GetStorageKey(ModuleId moduleId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(moduleId.Value, nameof(moduleId));
        // Keep the identity-based scheme separate from previous type-keyed estimates.
        return $"V3-{GetHash(moduleId.Value)}";
    }

    private static string GetHash(string value) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)));

    private static void TryDelete(FileInfo file)
    {
        try
        {
            file.Delete();
        }
        catch (Exception ex) when (IsFileAccessException(ex))
        {
            // Best-effort pruning. A locked cache entry can be retried on the next process run.
        }
    }

    private static bool IsFileAccessException(Exception exception) =>
        exception is IOException or UnauthorizedAccessException or System.Security.SecurityException;

    private async Task<EstimatedTimeEntry?> ReadEntryAsync(string fileName, CancellationToken cancellationToken)
    {
        var path = Path.Combine(_directory, fileName);
        try
        {
            if (!File.Exists(path))
            {
                return null;
            }

            var contents = await File.ReadAllTextAsync(path, cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize(contents, EstimatedTimeJsonContext.Default.EstimatedTimeEntry);
        }
        catch (Exception ex) when (IsFileAccessException(ex) || ex is JsonException)
        {
            return null;
        }
    }

    private async Task SaveEntryAsync(EstimatedTimeEntry entry, string fileName, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(_directory);

        var path = Path.Combine(_directory, fileName);
        var temporaryPath = Path.Combine(_directory, $".{Guid.NewGuid():N}.tmp");

        try
        {
            await File.WriteAllTextAsync(
                    temporaryPath,
                    JsonSerializer.Serialize(entry, EstimatedTimeJsonContext.Default.EstimatedTimeEntry),
                    cancellationToken)
                .ConfigureAwait(false);

            // Replace atomically so concurrent pipelines never read a partially written entry.
            File.Move(temporaryPath, path, overwrite: true);
        }
        catch (Exception ex) when (IsFileAccessException(ex) && File.Exists(path))
        {
            // Another pipeline replaced the entry at the same time; its measurement is equally valid.
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                TryDelete(new FileInfo(temporaryPath));
            }
        }
    }

    private sealed record SubModuleFileIndex(
        IReadOnlyDictionary<string, FileInfo[]> FilesByModule,
        long CreatedTimestamp);
}
