using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal class FileSystemModuleEstimatedTimeProvider : IModuleEstimatedTimeProvider
{
    private readonly string _directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ModularPipelines", "EstimatedTimes");

    public async Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType)
    {
        var fileName = $"{moduleType.FullName}.txt";
        return await GetEstimatedTimeAsync(fileName).ConfigureAwait(false);
    }

    public async Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration)
    {
        var fileName = $"{moduleType.FullName}.txt";

        await SaveModuleTimeAsync(duration, fileName).ConfigureAwait(false);
    }

    public async Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType)
    {
        var directoryInfo = new DirectoryInfo(_directory);

        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }

        directoryInfo.Create();

        var paths = directoryInfo
            .EnumerateFiles("*.txt", SearchOption.TopDirectoryOnly)
            .Where(x => x.Name.StartsWith($"Mod-{moduleType.FullName}"))
            .ToList();

        var subModuleEstimations = await paths.ToAsyncProcessorBuilder()
            .SelectAsync(async file =>
            {
                try
                {
                    var name = Path.GetFileNameWithoutExtension(file.FullName).Split("-Sub-")[1];
                    var time = await GetEstimatedTimeAsync(file.FullName).ConfigureAwait(false);
                    return new SubModuleEstimation(name, time);
                }
                catch
                {
                    File.Delete(file.FullName);
                    return null;
                }
            })
            .ProcessInParallel();

        return subModuleEstimations.OfType<SubModuleEstimation>();
    }

    public async Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation)
    {
        var fileName = $"Mod-{moduleType.FullName}-Sub-{subModuleEstimation.SubModuleName}.txt";

        await SaveModuleTimeAsync(subModuleEstimation.EstimatedDuration, fileName).ConfigureAwait(false);
    }

    private async Task<TimeSpan> GetEstimatedTimeAsync(string fileName)
    {
        var path = Path.Combine(_directory, fileName);

        if (File.Exists(path))
        {
            var contents = await File.ReadAllTextAsync(path).ConfigureAwait(false);
            return TimeSpan.Parse(contents);
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