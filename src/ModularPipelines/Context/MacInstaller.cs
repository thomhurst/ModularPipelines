using ModularPipelines.Models;
using ModularPipelines.Options.Mac;

namespace ModularPipelines.Context;

internal class MacInstaller : IMacInstaller
{
    private readonly ICommand _command;

    public MacInstaller(ICommand command)
    {
        _command = command;
    }

    public virtual async Task<CommandResult> InstallFromBrew(MacBrewOptions macBrewOptions)
    {
        return await _command.ExecuteCommandLineTool(macBrewOptions);
    }
}