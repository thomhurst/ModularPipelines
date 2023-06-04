using CliWrap.Buffered;
using ModularPipelines.Attributes;
using ModularPipelines.Command.Exceptions;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.NuGet.Extensions;
using ModularPipelines.NuGet.Options;

namespace ModularPipelines.Build.Modules.LocalMachine;

[DependsOn<CreateLocalNugetFolderModule>]
public class AddLocalNugetSourceModule : Module<BufferedCommandResult>
{
    public override Task<bool> ShouldIgnoreFailures(IModuleContext context, Exception exception)
    {
        return Task.FromResult(exception is CommandException commandException &&
                               commandException.CommandResult.StandardOutput.Contains("The name specified has already been added to the list of available package sources"));
    }

    protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var localNugetPathResult = await GetModule<CreateLocalNugetFolderModule>();
        
        return await context.NuGet()
            .AddSource(new NuGetSourceOptions(new Uri(localNugetPathResult.Value!), "ModularPipelinesLocalNuGet"));
    }
}