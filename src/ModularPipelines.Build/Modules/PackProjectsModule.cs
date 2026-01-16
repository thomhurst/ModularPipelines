using EnumerableAsyncProcessor.Extensions;
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
[DependsOn<FindProjectDependenciesModule>]
[DependsOn<RunUnitTestsModule>]
[RunOnLinuxOnly]
public class PackProjectsModule : Module<CommandResult[]>
{
    protected override async Task<CommandResult[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var packageVersion = context.GetModule<NugetVersionGeneratorModule, string>();

        var projectFiles = context.GetModule<FindProjectDependenciesModule, FindProjectDependenciesModule.ProjectDependencies>();


        var dependencies = await projectFiles.ValueOrDefault!.Dependencies
            .ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => await Pack(context, cancellationToken, projectFile, packageVersion))
            .ProcessOneAtATime();
        
        var others = await projectFiles.ValueOrDefault!.Others
            .ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => await Pack(context, cancellationToken, projectFile, packageVersion))
            .ProcessInParallel();

        return dependencies.Concat(others).ToArray();
    }

    private static async Task<CommandResult> Pack(IModuleContext context, CancellationToken cancellationToken, File projectFile, ModuleResult<string> packageVersion)
    {
        return await context.DotNet().Pack(new DotNetPackOptions
        {
            ProjectSolution = projectFile.Path,
            Configuration = "Release",
            IncludeSource = !projectFile.Path.Contains("Analyzer"),
            NoBuild = true,
            NoRestore = true,
            Properties = new List<KeyValue>
            {
                ("PackageVersion", packageVersion.ValueOrDefault!),
                ("Version", packageVersion.ValueOrDefault!),
            },
        }, cancellationToken: cancellationToken);
    }
}