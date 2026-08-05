using ModularPipelines.Context.Domains.Installers;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Context.Linux;
using ModularPipelines.Models;
using ModularPipelines.Options.Linux;
using ModularPipelines.Options.Linux.AptGet;

namespace ModularPipelines.Context;

internal class LinuxInstaller : ILinuxInstallerContext
{
    private readonly IAptGet _aptGet;
    private readonly ICommandContext _command;

    public LinuxInstaller(ICommandContext command, IAptGet aptGet)
    {
        _command = command;
        _aptGet = aptGet;
    }

    public virtual async Task<CommandResult> InstallFromDpkgAsync(
        DpkgInstallOptions options,
        CancellationToken cancellationToken = default)
    {
        var linuxInstallationResult = await _command.ExecuteCommandLineToolAsync(
            options,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        await _aptGet.InstallAsync(new AptGetInstallOptions
        {
            FixBroken = true,
        }, cancellationToken).ConfigureAwait(false);

        return linuxInstallationResult;
    }
}
