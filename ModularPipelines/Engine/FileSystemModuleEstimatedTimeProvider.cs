namespace ModularPipelines.Engine;

internal class FileSystemModuleEstimatedTimeProvider : IModuleEstimatedTimeProvider
{
    private readonly string _directory;

    public FileSystemModuleEstimatedTimeProvider()
    {
        _directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ModularPipelines", "EstimatedTimes");
    }

    public async Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType)
    {
        var fileName = $"{moduleType.FullName}.txt";
        return await GetEstimatedTimeAsync(fileName);
    }

    private async Task<TimeSpan> GetEstimatedTimeAsync(string fileName)
    {
        var path = Path.Combine(_directory, fileName);

        try
        {
            if (File.Exists(path))
            {
                var contents = await File.ReadAllTextAsync(path);
                return TimeSpan.Parse(contents);
            }
        }
        catch
        {
            // Ignored
        }

        // Some default fallback. We can't estimate for now so we'll estimate next time.
        return TimeSpan.FromMinutes(2);
    }

    public async Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration)
    {
        var fileName = $"{moduleType.FullName}.txt";

        await SaveModuleTimeAsync(duration, fileName);
    }

    private async Task SaveModuleTimeAsync(TimeSpan duration, string fileName)
    {
        try
        {
            Directory.CreateDirectory(_directory);

            var path = Path.Combine(_directory, fileName);

            await File.WriteAllTextAsync(path, duration.ToString());
        }
        catch
        {
            // Ignored
        }
    }

    public async Task<TimeSpan> GetSubModuleEstimatedTimeAsync(Type moduleType, string subModuleName)
    {
        var fileName = $"{moduleType.FullName}-{subModuleName}.txt";
        return await GetEstimatedTimeAsync(fileName);
    }

    public async Task SaveSubModuleTimeAsync(Type moduleType, string subModuleName, TimeSpan duration)
    {
        var fileName = $"{moduleType.FullName}-{subModuleName}.txt";

        await SaveModuleTimeAsync(duration, fileName);
    }
}
