using ModularPipelines.Models;
using ModularPipelines.Options.Windows;

namespace ModularPipelines.Context.Domains.Installers;

/// <summary>
/// Provides methods for installing software on Windows systems.
/// </summary>
public interface IWindowsInstallerContext
{
    /// <summary>
    /// Installs software from an MSI (Windows Installer) package.
    /// </summary>
    /// <param name="msiInstallerOptions">The options specifying the MSI file path and installation parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="CommandResult"/> containing the result of the MSI installation command.</returns>
    Task<CommandResult> InstallMsiAsync(
        MsiInstallerOptions msiInstallerOptions,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Installs software from an executable installer.
    /// </summary>
    /// <param name="exeInstallerOptions">The options specifying the executable file path and installation parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="CommandResult"/> containing the result of the executable installation command.</returns>
    Task<CommandResult> InstallExeAsync(
        ExeInstallerOptions exeInstallerOptions,
        CancellationToken cancellationToken = default);
}
