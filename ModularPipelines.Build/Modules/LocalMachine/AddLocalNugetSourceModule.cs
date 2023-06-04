using CliWrap.Buffered;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.NuGet.Extensions;
using ModularPipelines.NuGet.Options;

namespace ModularPipelines.Build.Modules.LocalMachine;

[DependsOn<CreateLocalNugetFolderModule>]
public class AddLocalNugetSourceModule : Module<BufferedCommandResult>
{
    public override bool ShouldIgnoreFailures(IModuleContext context) => true;

    protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var localNugetPathResult = await GetModule<CreateLocalNugetFolderModule>();
        
        return await context.NuGet()
            .AddSource(new NuGetSourceOptions(new Uri(localNugetPathResult.Value!), "ModularPipelinesLocalNuGet"));
    }
}