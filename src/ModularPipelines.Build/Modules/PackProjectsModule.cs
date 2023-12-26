using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[DependsOn<NugetVersionGeneratorModule>]
[DependsOn<PackageFilesRemovalModule>]
[DependsOn<CodeFormattedNicelyModule>]
[DependsOn<FindProjectDependenciesModule>]

[DependsOn<GetChangedFilesInPullRequest>]
[RunOnLinuxOnly]
public class PackProjectsModule : Module<CommandResult[]>
{
    /// <inheritdoc/>
    protected override async Task<CommandResult[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var packageVersion = await GetModule<NugetVersionGeneratorModule>();

        var projectFiles = await GetModule<FindProjectDependenciesModule>();

        var changesFiles = await GetModule<GetChangedFilesInPullRequest>();
        
        var dependencies = await projectFiles.Value!.Dependencies
            .ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => await Pack(context, cancellationToken, projectFile, packageVersion))
            .ProcessOneAtATime();

        var gitVersioningInformation = await context.Git().Versioning.GetGitVersioningInformation();
        
        var others = await projectFiles.Value!.Others
            .Where(x =>
            {
                if (gitVersioningInformation.BranchName == "main")
                {
                    return true;
                }
                
                return ProjectHasChanged(x,
                    changesFiles.Value?.Select(x => new File(x.FileName)).ToList() ?? new List<File>(), context);
            })
            .ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => await Pack(context, cancellationToken, projectFile, packageVersion))
            .ProcessInParallel();

        return dependencies.Concat(others).ToArray();
    }

    private bool ProjectHasChanged(File projectFile, IEnumerable<File> changedFiles,
        IPipelineContext context)
    {
        var projectDirectory = projectFile.Folder!;

        if (!changedFiles.Any(x => x.Path.Contains(projectDirectory.Path)))
        {
            context.Logger.LogInformation("{Project} has not changed so not packing it", projectFile.Name);
            return false;
        }
        
        context.Logger.LogInformation("{Project} has changed so packing it", projectFile.Name);

        return true;
    }

    private static async Task<CommandResult> Pack(IPipelineContext context, CancellationToken cancellationToken, File projectFile, ModuleResult<string> packageVersion)
    {
        return await context.DotNet().Pack(new DotNetPackOptions
        {
            TargetPath = projectFile.Path,
            Configuration = Configuration.Release,
            IncludeSource = !projectFile.Path.Contains("Analyzer"),
            NoRestore = true,
            Properties = new List<string>
            {
                $"PackageVersion={packageVersion.Value}",
                $"Version={packageVersion.Value}",
            },
        }, cancellationToken);
    }
}