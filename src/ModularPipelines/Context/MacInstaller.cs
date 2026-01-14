using ModularPipelines.Context.Domains.Installers;
using ModularPipelines.Models;
using ModularPipelines.Options.Mac;

namespace ModularPipelines.Context;

internal class MacInstaller : IMacInstaller, IMacInstallerContext
{
    private readonly ICommand _command;

    public MacInstaller(ICommand command)
    {
        _command = command;
    }

    public virtual async Task<CommandResult> InstallFromBrew(MacBrewOptions macBrewOptions)
    {
        return await _command.ExecuteCommandLineTool(macBrewOptions).ConfigureAwait(false);
    }
}