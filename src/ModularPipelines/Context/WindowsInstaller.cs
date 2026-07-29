using ModularPipelines.Context.Domains.Installers;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Options.Windows;

namespace ModularPipelines.Context;

internal class WindowsInstaller : IWindowsInstallerContext
{
    private readonly ICommandContext _command;

    public WindowsInstaller(ICommandContext command)
    {
        _command = command;
    }

    public virtual async Task<CommandResult> InstallMsiAsync(MsiInstallerOptions msiInstallerOptions)
    {
        return await _command.ExecuteCommandLineToolAsync(msiInstallerOptions).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> InstallExeAsync(ExeInstallerOptions exeInstallerOptions)
    {
        return await _command.ExecuteCommandLineToolAsync(exeInstallerOptions).ConfigureAwait(false);
    }
}
