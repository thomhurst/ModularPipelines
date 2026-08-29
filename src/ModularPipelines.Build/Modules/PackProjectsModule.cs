using EnumerableAsyncProcessor.Extensions;
using Microsoft.Build.Construction;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Build.Modules;

[DependsOn<BuildSolutionsModule>(Optional = true)]
[DependsOn<NugetVersionGeneratorModule>]
[DependsOn<PackageFilesRemovalModule>]
[DependsOn<FindProjectDependenciesModule>]
[DependsOn<RunAllUnitTestsModule>]
[RunIfAll<ModularPipelines.Conditions.OnLinux>]
public class PackProjectsModule : Module<CommandResult[]>
{
    protected override async Task<CommandResult[]> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var packageVersion = await context.GetModule<NugetVersionGeneratorModule>();

        var projectFiles = await context.GetModule<FindProjectDependenciesModule>();


        var dependencies = await projectFiles.Value.Dependencies
            .ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => await Pack(context, cancellationToken, projectFile, packageVersion.Value))
            .ProcessOneAtATime();

        var others = await projectFiles.Value.Others
            .ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => await Pack(context, cancellationToken, projectFile, packageVersion.Value))
            .ProcessInParallel();

        return dependencies.Concat(others).ToArray();
    }

    private static async Task<CommandResult> Pack(IModuleContext context, CancellationToken cancellationToken, FilePath projectFile, string packageVersion)
    {
        var effectiveVersion = GetEffectiveVersion(projectFile, packageVersion);

        return await context.Tools.DotNet.PackAsync(new DotNetPackOptions
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

    private static string GetEffectiveVersion(FilePath projectFile, string baseVersion)
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