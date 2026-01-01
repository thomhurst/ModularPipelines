using ModularPipelines.Models;
using ModularPipelines.Options.Linux;

namespace ModularPipelines.Context;

/// <summary>
/// Provides methods for installing software on Linux systems.
/// </summary>
public interface ILinuxInstaller
{
    /// <summary>
    /// Installs a Debian package using dpkg.
    /// </summary>
    /// <param name="options">The options specifying the package file and installation parameters.</param>
    /// <returns>A <see cref="CommandResult"/> containing the result of the dpkg installation command.</returns>
    Task<CommandResult> InstallFromDpkg(DpkgInstallOptions options);
}