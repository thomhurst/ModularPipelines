using ModularPipelines.Models;
using ModularPipelines.Options.Mac;

namespace ModularPipelines.Context;

public interface IMacInstaller
{
    Task<CommandResult> InstallFromBrew(MacBrewOptions macBrewOptions);
}