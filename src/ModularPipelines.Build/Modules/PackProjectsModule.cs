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
 DependsOn<FindProjectsModule>,
 DependsOn<BuildProjectsModule>]
public class PackProjectsModule : Module<CommandResult[]>
{
    protected override async Task<ModuleResult<CommandResult[]>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var packageVersion = await GetModule<NugetVersionGeneratorModule>();

        var projectFiles = await GetModule<FindProjectsModule>();
        
        return await projectFiles.Value!
            .ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => await Pack(context, cancellationToken, projectFile, packageVersion))
            .ProcessOneAtATime();
    }

    private static async Task<CommandResult> Pack(IModuleContext context, CancellationToken cancellationToken, File projectFile, ModuleResult<string> packageVersion)
    {
        return await context.DotNet().Pack(new DotNetPackOptions
        {
            TargetPath = projectFile.Path,
            IncludeSource = !projectFile.Path.Contains("Analyzer"),
            Properties = new List<string>
            {
                $"PackageVersion={packageVersion.Value}",
                $"Version={packageVersion.Value}"
            }
        }, cancellationToken);
    }
}
