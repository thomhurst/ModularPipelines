using ModularPipelines.Models;
using ModularPipelines.Options.Linux;

namespace ModularPipelines.Context.Domains.Installers;

/// <summary>
/// Provides methods for installing software on Linux systems.
/// </summary>
public interface ILinuxInstallerContext
{
    /// <summary>
    /// Installs a Debian package using dpkg.
    /// </summary>
    /// <param name="options">The options specifying the package file and installation parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="CommandResult"/> containing the result of the dpkg installation command.</returns>
    Task<CommandResult> InstallFromDpkgAsync(
        DpkgInstallOptions options,
        CancellationToken cancellationToken = default);
}
