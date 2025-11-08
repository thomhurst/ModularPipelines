using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;

namespace ModularPipelines.Build.Modules.LocalMachine;

[DependsOn<AddLocalNugetSourceModule>]
[DependsOn<PackagePathsParserModule>]
[DependsOn<CreateLocalNugetFolderModule>]
[RunOnLinuxOnly]
public class UploadPackagesToLocalNuGetModule : ModuleNew<CommandResult[]>, IModuleLifecycle
{
    /// <inheritdoc/>
    public async Task OnBeforeExecuteAsync(IPipelineContext context)
    {
        var packagePaths = await context.GetModuleAsync<PackagePathsParserModule>();

        foreach (var packagePath in packagePaths.Value!)
        {
            context.Logger.LogInformation("[Local Directory] Uploading {File}", packagePath);
        }
    }

    /// <inheritdoc/>
    public Task OnAfterExecuteAsync(IPipelineContext context, object? result, Exception? exception)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override async Task<CommandResult[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var localRepoLocation = await context.GetModuleAsync<CreateLocalNugetFolderModule>();
        var packagePaths = await context.GetModuleAsync<PackagePathsParserModule>();

        return await packagePaths.Value!
            .SelectAsync(async nugetFile => await context.DotNet().Nuget.Push(new DotNetNugetPushOptions
            {
                Path = nugetFile,
                Source = localRepoLocation.Value.AssertExists(),
            }, cancellationToken), cancellationToken: cancellationToken)
            .ProcessOneAtATime();
    }
}