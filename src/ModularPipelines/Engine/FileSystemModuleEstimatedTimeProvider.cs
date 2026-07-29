using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal class FileSystemModuleEstimatedTimeProvider : IModuleEstimatedTimeProvider
{
    private static readonly TimeSpan CacheRetention = TimeSpan.FromDays(90);
    private static readonly TimeSpan IndexRefreshInterval = TimeSpan.FromMinutes(1);

    private readonly object _subModuleIndexLock = new();
    private readonly string _directory;
    private readonly TimeProvider _timeProvider;
    private IReadOnlyDictionary<string, FileInfo[]>? _subModuleFilesByModule;
    private long _subModuleIndexExpiresAtTicks;

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

    public async Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType)
    {
        var fileName = $"{GetModuleName(moduleType)}.txt";
        return await GetEstimatedTimeAsync(fileName).ConfigureAwait(false);
    }

    public async Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration)
    {
        var fileName = $"{GetModuleName(moduleType)}.txt";

        await SaveModuleTimeAsync(duration, fileName).ConfigureAwait(false);
    }

    public async Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType)
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

                    var name = fileNameWithoutExtension[(subIndex + 5)..]; // 5 = length of "-Sub-"
                    var time = await GetEstimatedTimeAsync(file.FullName).ConfigureAwait(false);
                    return new SubModuleEstimation(name, time);
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Security.SecurityException)
                {
                    // File access error (locked, permissions, etc.) - skip gracefully without deleting
                    return null;
                }
            })
            .ProcessInParallel();

        return subModuleEstimations.OfType<SubModuleEstimation>();
    }

    public async Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation)
    {
        var fileName = $"Mod-{GetModuleName(moduleType)}-Sub-{subModuleEstimation.SubModuleName}.txt";

        await SaveModuleTimeAsync(subModuleEstimation.EstimatedDuration, fileName).ConfigureAwait(false);

        lock (_subModuleIndexLock)
        {
            Volatile.Write(ref _subModuleFilesByModule, null);
            Volatile.Write(ref _subModuleIndexExpiresAtTicks, 0);
        }
    }

    private IReadOnlyDictionary<string, FileInfo[]> GetSubModuleFilesByModule()
    {
        var now = _timeProvider.GetUtcNow();
        var filesByModule = Volatile.Read(ref _subModuleFilesByModule);
        if (filesByModule is not null
            && now.UtcTicks < Volatile.Read(ref _subModuleIndexExpiresAtTicks))
        {
            return filesByModule;
        }

        lock (_subModuleIndexLock)
        {
            now = _timeProvider.GetUtcNow();
            filesByModule = _subModuleFilesByModule;
            if (filesByModule is not null
                && now.UtcTicks < _subModuleIndexExpiresAtTicks)
            {
                return filesByModule;
            }

            filesByModule = BuildSubModuleFileIndex(now);
            Volatile.Write(
                ref _subModuleIndexExpiresAtTicks,
                now.Add(IndexRefreshInterval).UtcTicks);
            Volatile.Write(ref _subModuleFilesByModule, filesByModule);
            return filesByModule;
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

    private static void TryDelete(FileInfo file)
    {
        try
        {
            file.Delete();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Security.SecurityException)
        {
            // Best-effort pruning. A locked cache entry can be retried on the next process run.
        }
    }

    private async Task<TimeSpan> GetEstimatedTimeAsync(string fileName)
    {
        var path = Path.Combine(_directory, fileName);

        if (File.Exists(path))
        {
            try
            {
                var contents = await File.ReadAllTextAsync(path).ConfigureAwait(false);
                return TimeSpan.Parse(contents);
            }
            catch (FormatException)
            {
                // File contains malformed content - return default fallback
                return TimeSpan.FromMinutes(2);
            }
        }

        // Some default fallback. We can't estimate for now so we'll estimate next time.
        return TimeSpan.FromMinutes(2);
    }

    private async Task SaveModuleTimeAsync(TimeSpan duration, string fileName)
    {
        Directory.CreateDirectory(_directory);

        var path = Path.Combine(_directory, fileName);

        await File.WriteAllTextAsync(path, duration.ToString()).ConfigureAwait(false);
    }
}
