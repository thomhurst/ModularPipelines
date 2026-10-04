using System.Runtime.CompilerServices;
using ModularPipelines;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;
using NugetVersionGeneratorModule = ModularPipelines.DocumentationSnippets.CurrentApiSnippets.NugetVersionGeneratorModule;

namespace ModularPipelines.DocumentationSnippets.SubModules;

[DependsOn<NugetVersionGeneratorModule>]
public class PackProjectsModule : Module<CommandResult[]>
{
    protected override async Task<CommandResult[]> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var packageVersion = await context.GetModule<NugetVersionGeneratorModule>();

        var repositoryInfo = await context.Tools.Git.Information.GetRequiredInfoAsync(cancellationToken);
        var projects = repositoryInfo.Root
            .GetFiles(x =>
                x.Extension == ".csproj" && !x.Name.Contains("test", StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        return await PackProjectsAsync(context, projects, packageVersion.Value, cancellationToken).ToArrayAsync(cancellationToken: cancellationToken);
    }

    private async IAsyncEnumerable<CommandResult> PackProjectsAsync(IModuleContext context, List<FilePath> projects, string packageVersion, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (var project in projects)
        {
            yield return await context.RunSubModuleAsync(project.Name, token => context.Tools.DotNet.PackAsync(new DotNetPackOptions
            {
                ProjectSolution = project.Path,
                Configuration = "Release",
                Properties =
                [
                    new KeyValue("PackageVersion", packageVersion),
                    new KeyValue("Version", packageVersion),
                ],
            }, cancellationToken: token), cancellationToken);
        }
    }
}
