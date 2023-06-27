namespace ModularPipelines.Engine;

internal class FileSystemModuleEstimatedTimeProvider : IModuleEstimatedTimeProvider
{
    private readonly string _directory;

    public FileSystemModuleEstimatedTimeProvider()
    {
        _directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ModularPipelines", "EstimatedTimes");
    }
    
    public async Task<TimeSpan> GetEstimatedTimeAsync(Type moduleType)
    {
        var path = Path.Combine(_directory, $"{moduleType.FullName}.txt");

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

    public async Task SaveTimeAsync(Type moduleType, TimeSpan duration)
    {
        try
        {
            Directory.CreateDirectory(_directory);
            
            var path = Path.Combine(_directory, $"{moduleType.FullName}.txt");
        
            await File.WriteAllTextAsync(path, duration.ToString());
        }
        catch
        {
            // Ignored
        }
    }
}