using Pipeline.NET.DotNet.Options;
using Pipeline.NET.Extensions;

namespace Pipeline.NET.DotNet.Modules;

public abstract class MultiDotNetModule : Module
{
    public MultiDotNetModule(IModuleContext context) : base(context)
    {
    }
    
    public abstract MultiDotNetModuleOptions Options { get; }

    public override async Task<ModuleResult?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var projects = GetProjects();

        var innerModules = projects
            .Select(path => new InternalDotNetCommandModule(Context, new DotNetCommandModuleOptions
            {
                Command = Options.Command,
                ExtraArguments = Options.ExtraArguments,
                WorkingDirectory = Options.WorkingDirectory,
                ProjectOrSolutionPath = path,
                AssertSuccess = Options.AssertSuccess
            }).StartProcessingModule());

        var moduleResults = await Task.WhenAll(innerModules);

        return moduleResults.ToModuleResult();
    }

    private IEnumerable<string> GetProjects()
    {
        var directoryToStartSearchFrom =
            new DirectoryInfo(Options.WorkingDirectory ?? Context.Environment.WorkingDirectory.FullName);

        return directoryToStartSearchFrom
            .EnumerateFiles()
            .Where(fileInfo => Options.ProjectsToInclude?.Invoke(fileInfo.FullName) ?? fileInfo.FullName.EndsWith(".csproj"))
            .Select(fileInfo => fileInfo.FullName)
            .ToArray();
    }
}