using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using TomLonghurst.EnumerableAsyncProcessor.Extensions;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[DependsOn<PackageFilesRemovalModule>, 
 DependsOn<CodeFormattedNicelyModule>, 
 DependsOn<FindProjectDependenciesModule>,
 DependsOn<RunUnitTestsModule>]
public class PackProjectsModule : Module<CommandResult[]>
{
    protected override async Task<CommandResult[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var packageVersion = await GetModule<NugetVersionGeneratorModule>();

        var projectFiles = await GetModule<FindProjectDependenciesModule>();
        
        var dependencies = await projectFiles.Value!.Dependencies
            .ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => await Pack(context, cancellationToken, projectFile, packageVersion))
            .ProcessOneAtATime();
        
        var others = await projectFiles.Value!.Others
            .ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => await Pack(context, cancellationToken, projectFile, packageVersion))
            .ProcessInParallel();
        
        return dependencies.Concat(others).ToArray();
    }

    private static async Task<CommandResult> Pack(IPipelineContext context, CancellationToken cancellationToken, File projectFile, ModuleResult<string> packageVersion)
    {
        return await context.DotNet().Pack(new DotNetPackOptions
        {
            TargetPath = projectFile.Path,
            Configuration = Configuration.Release,
            IncludeSource = !projectFile.Path.Contains("Analyzer"),
            Properties = new List<string>
            {
                $"PackageVersion={packageVersion.Value}",
                $"Version={packageVersion.Value}"
            }
        }, cancellationToken);
    }
}
