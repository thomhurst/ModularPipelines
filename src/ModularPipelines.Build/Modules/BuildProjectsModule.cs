using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using TomLonghurst.EnumerableAsyncProcessor.Extensions;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[DependsOn<FindProjectDependenciesModule>]
public class BuildProjectsModule : Module<CommandResult[]>
{
    protected override async Task<ModuleResult<CommandResult[]>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var packageVersion = await GetModule<NugetVersionGeneratorModule>();

        var projectFiles = await GetModule<FindProjectDependenciesModule>();
        
        var dependencies = await projectFiles.Value!.Dependencies
            .ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => await Build(context, cancellationToken, projectFile, packageVersion))
            .ProcessOneAtATime();
        
        var others = await projectFiles.Value!.Others
            .ToAsyncProcessorBuilder()
            .SelectAsync(async projectFile => await Build(context, cancellationToken, projectFile, packageVersion))
            .ProcessInParallel();
        
        return dependencies.Concat(others).ToArray();
    }

    private static async Task<CommandResult> Build(IModuleContext context, CancellationToken cancellationToken, File projectFile, ModuleResult<string> packageVersion)
    {
        return await context.DotNet().Build(new DotNetBuildOptions
        {
            TargetPath = projectFile.Path
        }, cancellationToken);
    }
}
