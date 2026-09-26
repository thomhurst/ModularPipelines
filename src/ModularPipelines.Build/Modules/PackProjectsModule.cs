using EnumerableAsyncProcessor.Extensions;
using Microsoft.Build.Construction;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[RequiresCapability("ci-master")]
[DependsOn<BuildSolutionsModule>(Optional = true)]
[DependsOn<NugetVersionGeneratorModule>]
[DependsOn<PackageFilesRemovalModule>]
[DependsOn<FindProjectDependenciesModule>]
[DependsOn<RunAllUnitTestsModule>]
[RunIf<ModularPipelines.OnLinux>]
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

        return [.. dependencies, .. others];
    }

    private static async Task<CommandResult> Pack(IModuleContext context, CancellationToken cancellationToken, FilePath projectFile, string packageVersion)
    {
        var projectRoot = ProjectRootElement.Open(projectFile.Path);
        var effectiveVersion = GetEffectiveVersion(projectRoot, packageVersion);
        var includeBuildOutput = projectRoot?.Properties
            .LastOrDefault(property => property.Name == "IncludeBuildOutput")?.Value;

        return await context.Tools.DotNet.PackAsync(new DotNetPackOptions
        {
            ProjectSolution = projectFile.Path,
            Configuration = "Release",
            // Content-only packages have no build output for a symbols package.
            IncludeSource = !projectFile.Path.Contains("Analyzer")
                && !string.Equals(includeBuildOutput, bool.FalseString, StringComparison.OrdinalIgnoreCase),
            NoBuild = true,
            NoRestore = true,
            Properties =
            [
                ("PackageVersion", effectiveVersion),
                ("Version", effectiveVersion),
            ],
        }, cancellationToken: cancellationToken);
    }

    private static string GetEffectiveVersion(ProjectRootElement? projectRoot, string baseVersion)
    {
        var versionSuffix = projectRoot?.Properties
            .FirstOrDefault(p => p.Name == "VersionSuffix")?.Value;

        if (!string.IsNullOrWhiteSpace(versionSuffix) && !baseVersion.Contains('-'))
        {
            return $"{baseVersion}-{versionSuffix}";
        }

        return baseVersion;
    }
}
