using System.Text;
using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Reporting;

namespace ModularPipelines.Engine;

internal class FileSystemModuleEstimatedTimeProvider : IModuleEstimatedTimeProvider
{
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

                    var encodedName = fileNameWithoutExtension[(subIndex + 5)..]; // 5 = length of "-Sub-"
                    var name = DecodeSubModuleName(encodedName);
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
        var moduleName = GetModuleName(moduleType);
        var encodedSubModuleName = EncodeSubModuleName(subModuleEstimation.SubModuleName);
        var fileName = $"Mod-{moduleName}-Sub-{encodedSubModuleName}.txt";

        await SaveModuleTimeAsync(subModuleEstimation.EstimatedDuration, fileName).ConfigureAwait(false);

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

    private sealed record SubModuleFileIndex(
        IReadOnlyDictionary<string, FileInfo[]> FilesByModule,
        long CreatedTimestamp);
}
