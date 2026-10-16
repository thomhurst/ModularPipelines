using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[RequiresCapability("ci-master")]
public class FindProjectsModule : Module<IReadOnlyList<FilePath>>
{
    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithAlwaysRun();

    protected override async Task<IReadOnlyList<FilePath>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var repositoryInfo = await context.Tools.Git.Information.GetInfoAsync(cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException("Git repository information is unavailable.");
        var gitRoot = repositoryInfo.Root.Path;

        return [.. File.ReadLines(Path.Combine(gitRoot, "PackProjects.txt"))
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrEmpty(line) && !line.StartsWith('#'))
            .Select(project => new FilePath(Path.Combine(gitRoot, project)))];
    }
}
