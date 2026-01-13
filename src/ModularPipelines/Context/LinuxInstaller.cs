using ModularPipelines.Context.Domains.Installers;
using ModularPipelines.Context.Linux;
using ModularPipelines.Models;
using ModularPipelines.Options.Linux;
using ModularPipelines.Options.Linux.AptGet;

namespace ModularPipelines.Context;

internal class LinuxInstaller : ILinuxInstaller, ILinuxInstallerContext
{
    private readonly IAptGet _aptGet;
    private readonly ICommand _command;

    public LinuxInstaller(ICommand command, IAptGet aptGet)
    {
        _command = command;
        _aptGet = aptGet;
    }

    public virtual async Task<CommandResult> InstallFromDpkg(DpkgInstallOptions options)
    {
        var linuxInstallationResult = await _command.ExecuteCommandLineTool(options).ConfigureAwait(false);

        await _aptGet.Install(new AptGetInstallOptions
        {
            FixBroken = true,
        }).ConfigureAwait(false);

        return linuxInstallationResult;
    }
}