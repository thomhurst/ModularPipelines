using ModularPipelines.Context.Domains.Installers;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Options.Mac;

namespace ModularPipelines.Context;

internal class MacInstaller : IMacInstallerContext
{
    private readonly ICommandContext _command;

    public MacInstaller(ICommandContext command)
    {
        _command = command;
    }

    public virtual async Task<CommandResult> InstallFromBrewAsync(
        MacBrewOptions macBrewOptions,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(
            macBrewOptions,
            cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
