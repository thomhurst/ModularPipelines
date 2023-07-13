using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Options.Mac;

namespace ModularPipelines.Context;

internal class MacInstaller : IMacInstaller
{
    private readonly ICommand _command;

    public MacInstaller(ICommand command)
    {
        _command = command;
    }

    public async Task<CommandResult> InstallFromBrew(MacBrewOptions macBrewOptions)
    {
        return await _command.ExecuteCommandLineTool(macBrewOptions);
    }
}