using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.NuGet.Extensions;
using ModularPipelines.NuGet.Options;

namespace ModularPipelines.Build.Modules.LocalMachine;

[DependsOn<AddLocalNugetSourceModule>]
public class UploadPackagesToLocalNuGetModule : Module<List<CommandResult>>
{
    protected override async Task OnBeforeExecute( IModuleContext context )
    {
        var packagePaths = await GetModule<PackagePathsParserModule>();

        foreach (var packagePath in packagePaths.Value!)
        {
            context.Logger.LogInformation( "[Local Directory] Uploading {File}", packagePath );
        }

        await base.OnBeforeExecute( context );
    }

    protected override async Task<ModuleResult<List<CommandResult>>?> ExecuteAsync( IModuleContext context, CancellationToken cancellationToken )
    {
        var localRepoLocation = await GetModule<CreateLocalNugetFolderModule>();
        var packagePaths = await GetModule<PackagePathsParserModule>();

        return await context.NuGet()
            .UploadPackages( new NuGetUploadOptions( packagePaths.Value!, new Uri( localRepoLocation.Value! ) ) );
    }
}
