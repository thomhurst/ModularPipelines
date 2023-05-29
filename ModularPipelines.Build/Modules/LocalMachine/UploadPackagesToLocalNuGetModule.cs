using CliWrap.Buffered;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.NuGet.Extensions;
using ModularPipelines.NuGet.Options;

namespace ModularPipelines.Build.Modules.LocalMachine;

[DependsOn<AddLocalNugetSourceModule>]
public class UploadPackagesToLocalNuGetModule : Module<List<BufferedCommandResult>>
{
    public UploadPackagesToLocalNuGetModule(IModuleContext context) : base(context)
    {
    }

    protected override async Task InitialiseAsync()
    {
        var packagePaths = await GetModule<PackagePathsParserModule>();
        
        foreach (var packagePath in packagePaths.Value!)
        {
            Context.Logger.LogInformation("Uploading {File}", packagePath);
        }
        
        await base.InitialiseAsync();
    }

    protected override async Task<ModuleResult<List<BufferedCommandResult>>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var localRepoLocation = await GetModule<CreateLocalNugetFolderModule>();
        var packagePaths = await GetModule<PackagePathsParserModule>();

        return await Context.NuGet()
            .UploadPackage(new NuGetUploadOptions(packagePaths.Value!, new Uri(localRepoLocation.Value!)));
    }
}