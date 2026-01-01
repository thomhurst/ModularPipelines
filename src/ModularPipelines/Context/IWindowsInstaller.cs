using ModularPipelines.Models;
using ModularPipelines.Options.Windows;

namespace ModularPipelines.Context;

/// <summary>
/// Provides methods for installing software on Windows systems.
/// </summary>
public interface IWindowsInstaller
{
    /// <summary>
    /// Installs software from an MSI (Windows Installer) package.
    /// </summary>
    /// <param name="msiInstallerOptions">The options specifying the MSI file path and installation parameters.</param>
    /// <returns>A <see cref="CommandResult"/> containing the result of the MSI installation command.</returns>
    Task<CommandResult> InstallMsi(MsiInstallerOptions msiInstallerOptions);

    /// <summary>
    /// Installs software from an executable installer.
    /// </summary>
    /// <param name="exeInstallerOptions">The options specifying the executable file path and installation parameters.</param>
    /// <returns>A <see cref="CommandResult"/> containing the result of the executable installation command.</returns>
    Task<CommandResult> InstallExe(ExeInstallerOptions exeInstallerOptions);
}