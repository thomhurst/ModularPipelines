using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules.LocalMachine;

[DependsOn<AddLocalNugetSourceModule>]
[DependsOn<PackagePathsParserModule>]
[DependsOn<CreateLocalNugetFolderModule>]
[RunOnLinuxOnly]
public class UploadPackagesToLocalNuGetModule : Module<CommandResult[]>
{
    /// <inheritdoc/>
    protected override async Task OnBeforeExecute(IPipelineContext context)
    {
        var packagePaths = await GetModule<PackagePathsParserModule>();

        foreach (var packagePath in packagePaths.Value!)
        {
            context.Logger.LogInformation("[Local Directory] Uploading {File}", packagePath);
        }

        await base.OnBeforeExecute(context);
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var localRepoLocation = await GetModule<CreateLocalNugetFolderModule>();
        var packagePaths = await GetModule<PackagePathsParserModule>();

        return await packagePaths.Value!
            .SelectAsync(async nugetFile => await context.DotNet().Nuget.Push(new DotNetNugetPushOptions
            {
                Path = nugetFile,
                Source = localRepoLocation.Value.AssertExists(),
            }, cancellationToken), cancellationToken: cancellationToken)
            .ProcessOneAtATime();
    }
}