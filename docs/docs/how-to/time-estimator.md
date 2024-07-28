---
title: Time Estimator
---

# Time Estimator

The time estimator is a class built by you, used to estimate times for modules for displaying in the console progress dialog. It isn't mandatory, but without it, estimated times will not be correct.

The idea is that on every run of a module, it takes note of how long it took to run, and then provides it to this class to save somewhere. That's up to you. A blob storage, a database, wherever.

Then on subsequent runs, it'll ask you for an estimated time for a module. You go and pull this back out of your database or wherever, and then pass it back to the framework.

## Example

```csharp
await PipelineHostBuilder.Create()
    .AddModule<Module1>()
    .AddModule<Module2>()
    .AddModule<Module3>()
    .AddModuleEstimatedTimeProvider<MyEstimatedTimeProvider>()
    .ExecutePipelineAsync();
```

```csharp
public class MyEstimatedTimeProvider : IModuleEstimatedTimeProvider
{
    private readonly string _directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ModularPipelines", "EstimatedTimes");

    public async Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType)
    {
        var fileName = $"{moduleType.FullName}.txt";
        return await GetEstimatedTimeAsync(fileName);
    }

    public async Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration)
    {
        var fileName = $"{moduleType.FullName}.txt";

        await SaveModuleTimeAsync(duration, fileName);
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
                    var time = await GetEstimatedTimeAsync(file.FullName);
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

        await SaveModuleTimeAsync(subModuleEstimation.EstimatedDuration, fileName);
    }

    private async Task<TimeSpan> GetEstimatedTimeAsync(string fileName)
    {
        var path = Path.Combine(_directory, fileName);

        if (File.Exists(path))
        {
            var contents = await File.ReadAllTextAsync(path);
            return TimeSpan.Parse(contents);
        }

        // Some default fallback. We can't estimate for now so we'll estimate next time.
        return TimeSpan.FromMinutes(2);
    }

    private async Task SaveModuleTimeAsync(TimeSpan duration, string fileName)
    {
        Directory.CreateDirectory(_directory);

        var path = Path.Combine(_directory, fileName);

        await File.WriteAllTextAsync(path, duration.ToString());
    }
}
```