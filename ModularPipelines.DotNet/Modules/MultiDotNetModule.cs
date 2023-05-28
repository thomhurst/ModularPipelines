using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ParallelOptions = ModularPipelines.DotNet.Options.ParallelOptions;

namespace ModularPipelines.DotNet.Modules;

public abstract class MultiDotNetModule : Module<ModuleResult<BufferedCommandResult>[]>
{
    protected MultiDotNetModule(IModuleContext context) : base(context)
    {
    }

    protected abstract MultiDotNetModuleOptions Options { get; set; }

    protected override async Task<ModuleResult<ModuleResult<BufferedCommandResult>[]>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var projects = GetProjects();

        var innerModules = projects
            .Select(path => new ExternalRunnableDotNetCommandModule(Context, new DotNetCommandModuleOptions
            {
                Command = Options.Command,
                ExtraArguments = Options.ExtraArguments,
                WorkingDirectory = Options.WorkingDirectory,
                TargetPath = path,
                Configuration = Options.Configuration,
                EnvironmentVariables = Options.EnvironmentVariables
            }))
            .ToArray();

        foreach (var internalDotNetCommandModule in innerModules)
        {
            if (Options.ParallelOptions == ParallelOptions.OneAtATime)
            {
                await internalDotNetCommandModule.StartProcessingModule();
            }
            else
            {
                _ = internalDotNetCommandModule.StartProcessingModule();
            }
        }


        var moduleResults = await Task.WhenAll(innerModules.Select(async x => await x));

        return moduleResults;
    }

    private IEnumerable<string> GetProjects()
    {
        var directoryToStartSearchFrom =
            new DirectoryInfo(Options.WorkingDirectory ?? Context.Environment.WorkingDirectory.FullName);

        return directoryToStartSearchFrom
            .EnumerateFiles("*", SearchOption.AllDirectories)
            .Where(fileInfo => Options.ProjectsToInclude?.Invoke(fileInfo.FullName) ?? fileInfo.FullName.EndsWith(".csproj"))
            .Select(fileInfo => fileInfo.FullName)
            .ToArray();
    }
}