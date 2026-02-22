using EnumerableAsyncProcessor.Extensions;
using Microsoft.Build.Construction;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[PinToMaster]
[DependsOn<BuildSolutionsModule>(Optional = true)]
[DependsOn<NugetVersionGeneratorModule>]
[DependsOn<PackageFilesRemovalModule>]
[DependsOn<FindProjectDependenciesModule>]
[DependsOn<RunUnitTestsModule>]
[RunOnLinuxOnly]
public class PackProjectsModule : Module<CommandResult[]>
{
    protected override async Task<CommandResult[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var packageVersion = await context.GetModule<NugetVersionGeneratorModule>();

        var projectFiles = await context.GetModule<FindProjectDependenciesModule>();


        var dependencies = await projectFiles.ValueOrDefault!.Dependencies
            .ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => await Pack(context, cancellationToken, projectFile, packageVersion.ValueOrDefault!))
            .ProcessOneAtATime();

        var others = await projectFiles.ValueOrDefault!.Others
            .ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => await Pack(context, cancellationToken, projectFile, packageVersion.ValueOrDefault!))
            .ProcessInParallel();

        return dependencies.Concat(others).ToArray();
    }

    private static async Task<CommandResult> Pack(IModuleContext context, CancellationToken cancellationToken, File projectFile, string packageVersion)
    {
        var effectiveVersion = GetEffectiveVersion(projectFile, packageVersion);

        return await context.DotNet().Pack(new DotNetPackOptions
        {
            ProjectSolution = projectFile.Path,
            Configuration = "Release",
            IncludeSource = !projectFile.Path.Contains("Analyzer"),
            NoBuild = true,
            NoRestore = true,
            Properties = new List<KeyValue>
            {
                ("PackageVersion", effectiveVersion),
                ("Version", effectiveVersion),
            },
        }, cancellationToken: cancellationToken);
    }

    private static string GetEffectiveVersion(File projectFile, string baseVersion)
    {
        var projectRoot = ProjectRootElement.Open(projectFile.Path);
        var versionSuffix = projectRoot?.Properties
            .FirstOrDefault(p => p.Name == "VersionSuffix")?.Value;

        if (!string.IsNullOrWhiteSpace(versionSuffix) && !baseVersion.Contains('-'))
        {
            return $"{baseVersion}-{versionSuffix}";
        }

        return baseVersion;
    }
}