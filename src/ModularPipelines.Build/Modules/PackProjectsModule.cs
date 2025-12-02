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
[DependsOn<CodeFormattedNicelyModule>(IgnoreIfNotRegistered = true)]
[DependsOn<FindProjectDependenciesModule>]
[DependsOn<ChangedFilesInPullRequestModule>]
[DependsOn<RunUnitTestsModule>]
[RunOnLinuxOnly]
public class PackProjectsModule : Module<CommandResult[]>
{
    public override async Task<CommandResult[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var packageVersion = context.GetModule<NugetVersionGeneratorModule, string>();

        var projectFiles = context.GetModule<FindProjectDependenciesModule, FindProjectDependenciesModule.ProjectDependencies>();

        var changedFiles = context.GetModule<ChangedFilesInPullRequestModule, IReadOnlyList<File>>();

        var dependencies = await projectFiles.Value!.Dependencies
            .ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => await Pack(context, cancellationToken, projectFile, packageVersion))
            .ProcessOneAtATime();

        var gitVersioningInformation = await context.Git().Versioning.GetGitVersioningInformation();

        var others = await projectFiles.Value!.Others
            .Where(x =>
            {
                if (changedFiles.SkipDecision.ShouldSkip)
                {
                    return true;
                }

                return ProjectHasChanged(x,
                    changedFiles.Value!, context);
            })
            .ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => await Pack(context, cancellationToken, projectFile, packageVersion))
            .ProcessInParallel();

        return dependencies.Concat(others).ToArray();
    }

    private bool ProjectHasChanged(File projectFile, IEnumerable<File> changedFiles,
        IModuleContext context)
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

    private static async Task<CommandResult> Pack(IModuleContext context, CancellationToken cancellationToken, File projectFile, ModuleResult<string> packageVersion)
    {
        return await context.DotNet().Pack(new DotNetPackOptions
        {
            ProjectSolution = projectFile.Path,
            Configuration = Configuration.Release,
            IncludeSource = !projectFile.Path.Contains("Analyzer"),
            NoRestore = true,
            Properties = new List<KeyValue>
            {
                ("PackageVersion", packageVersion.Value!),
                ("Version", packageVersion.Value!),
            },
        }, cancellationToken);
    }
}