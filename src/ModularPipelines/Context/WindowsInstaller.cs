using ModularPipelines.Models;
using ModularPipelines.Options.Windows;

namespace ModularPipelines.Context;

internal class WindowsInstaller : IWindowsInstaller
{
    private readonly ICommand _command;

    public WindowsInstaller(ICommand command)
    {
        _command = command;
    }

    public virtual async Task<CommandResult> InstallMsi(MsiInstallerOptions msiInstallerOptions)
    {
        return await _command.ExecuteCommandLineTool(msiInstallerOptions).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> InstallExe(ExeInstallerOptions exeInstallerOptions)
    {
        return await _command.ExecuteCommandLineTool(exeInstallerOptions).ConfigureAwait(false);
    }
}