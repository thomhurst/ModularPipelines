using System.Globalization;
using System.Text;
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

    private const string EncodedSubModuleNamePrefix = "B64-";

    private static readonly TimeSpan CacheRetention = TimeSpan.FromDays(90);
    private static readonly TimeSpan IndexRefreshInterval = TimeSpan.FromMinutes(1);

    private readonly object _subModuleIndexLock = new();
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

    public async Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType, CancellationToken cancellationToken = default)
    {
        var fileName = $"{GetModuleName(moduleType)}.txt";
        return await GetEstimatedTimeAsync(fileName, cancellationToken).ConfigureAwait(false);
    }

    public async Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration, CancellationToken cancellationToken = default)
    {
        var fileName = $"{GetModuleName(moduleType)}.txt";

        await SaveModuleTimeAsync(duration, fileName, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(
        Type moduleType,
        CancellationToken cancellationToken = default)
    {
        var filesByModule = GetSubModuleFilesByModule();
        var moduleName = GetModuleName(moduleType);
        var paths = filesByModule.GetValueOrDefault(moduleName, []);

        var subModuleEstimations = await paths.ToAsyncProcessorBuilder()
            .SelectAsync(async file =>
            {
                try
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FullName);
                    var subIndex = fileNameWithoutExtension.IndexOf("-Sub-", StringComparison.Ordinal);

                    if (subIndex < 0)
                    {
                        // File doesn't match expected naming pattern - skip gracefully
                        return null;
                    }

                    var encodedName = fileNameWithoutExtension[(subIndex + 5)..]; // 5 = length of "-Sub-"
                    var name = DecodeSubModuleName(encodedName);
                    var time = await GetEstimatedTimeAsync(file.FullName, cancellationToken).ConfigureAwait(false);
                    return new SubModuleEstimation(name, time);
                }
                catch (Exception ex) when (IsFileAccessException(ex))
                {
                    // File access error (locked, permissions, etc.) - skip gracefully without deleting
                    return null;
                }
            })
            .ProcessInParallel();

        return subModuleEstimations.OfType<SubModuleEstimation>();
    }

    public async Task SaveSubModuleTimeAsync(
        Type moduleType,
        SubModuleEstimation subModuleEstimation,
        CancellationToken cancellationToken = default)
    {
        var moduleName = GetModuleName(moduleType);
        var encodedSubModuleName = EncodeSubModuleName(subModuleEstimation.SubModuleName);
        var fileName = $"Mod-{moduleName}-Sub-{encodedSubModuleName}.txt";

        await SaveModuleTimeAsync(subModuleEstimation.EstimatedDuration, fileName, cancellationToken).ConfigureAwait(false);

        lock (_subModuleIndexLock)
        {
            Volatile.Write(ref _subModuleFileIndex, null);
        }
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

    private IReadOnlyDictionary<string, FileInfo[]> BuildSubModuleFileIndex(DateTimeOffset now)
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

    private static string GetModuleName(Type moduleType) => moduleType.FullName ?? moduleType.Name;

    private static string EncodeSubModuleName(string name)
    {
        var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(name))
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');

        return $"{EncodedSubModuleNamePrefix}{base64}";
    }

    private static string DecodeSubModuleName(string name)
    {
        if (!name.StartsWith(EncodedSubModuleNamePrefix, StringComparison.Ordinal))
        {
            return name;
        }

        var base64 = name[EncodedSubModuleNamePrefix.Length..]
            .Replace('-', '+')
            .Replace('_', '/');
        base64 = base64.PadRight(base64.Length + ((4 - (base64.Length % 4)) % 4), '=');

        try
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        }
        catch (FormatException)
        {
            return name;
        }
    }

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

    private async Task<TimeSpan> GetEstimatedTimeAsync(string fileName, CancellationToken cancellationToken)
    {
        var path = Path.Combine(_directory, fileName);

        try
        {
            if (!File.Exists(path))
            {
                // We can't estimate for now, so we'll estimate next time.
                return DefaultEstimate;
            }

            var contents = await File.ReadAllTextAsync(path, cancellationToken).ConfigureAwait(false);
            return TimeSpan.TryParse(contents, CultureInfo.InvariantCulture, out var estimate)
                ? estimate
                : DefaultEstimate;
        }
        catch (Exception ex) when (IsFileAccessException(ex))
        {
            // Locked, removed or unreadable entry - fall back rather than fail the module.
            return DefaultEstimate;
        }
    }

    private async Task SaveModuleTimeAsync(TimeSpan duration, string fileName, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(_directory);

        var path = Path.Combine(_directory, fileName);
        var temporaryPath = Path.Combine(_directory, $".{Guid.NewGuid():N}.tmp");

        try
        {
            await File.WriteAllTextAsync(
                    temporaryPath,
                    duration.ToString("c", CultureInfo.InvariantCulture),
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
