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
    public AddLocalNugetSourceModule(IModuleContext context) : base(context)
    {
    }

    protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var localNugetPathResult = await GetModule<CreateLocalNugetFolderModule>();
        
        return await Context.NuGet()
            .AddSource(new NuGetSourceOptions(new Uri(localNugetPathResult.Value!), "ModularPipelinesLocalNuGet"));
    }
}