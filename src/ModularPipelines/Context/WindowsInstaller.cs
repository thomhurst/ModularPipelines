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

    public virtual async Task<CommandResult> InstallMsiAsync(
        MsiInstallerOptions msiInstallerOptions,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(
            msiInstallerOptions,
            cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> InstallExeAsync(
        ExeInstallerOptions exeInstallerOptions,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(
            exeInstallerOptions,
            cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
