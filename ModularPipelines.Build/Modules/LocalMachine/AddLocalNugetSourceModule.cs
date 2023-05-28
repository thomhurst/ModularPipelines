using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Modules;
using ModularPipelines.DotNet.Options;

namespace ModularPipelines.Build.Modules.LocalMachine;

[DependsOn<CreateLocalNugetFolderModule>]
public class AddLocalNugetSourceModule : DotNetCommandModule
{
    public AddLocalNugetSourceModule(IModuleContext context) : base(context)
    {
    }

    protected override DotNetCommandModuleOptions Options { get; set; } = null!;

    protected override async Task InitialiseAsync()
    {
        var localNugetPathResult = await GetModule<CreateLocalNugetFolderModule>();

        Options = new DotNetCommandModuleOptions
        {
            Command = new[] {"nuget", "add", "source", localNugetPathResult.Value!, "-n", "ModularPipelinesLocalNuGet"}
        };
        
        await base.InitialiseAsync();
    }
}