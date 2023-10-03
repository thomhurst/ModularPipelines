using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.NuGet.Extensions;
using ModularPipelines.NuGet.Options;

namespace ModularPipelines.Build.Modules.LocalMachine;

[DependsOn<AddLocalNugetSourceModule>]
[DependsOn<PackagePathsParserModule>]
[DependsOn<CreateLocalNugetFolderModule>]
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

        return await context.NuGet()
            .UploadPackages(new NuGetUploadOptions(packagePaths.Value!.AsPaths(), new Uri(localRepoLocation.Value!)));
    }
}
