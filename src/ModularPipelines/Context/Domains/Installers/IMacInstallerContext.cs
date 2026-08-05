using ModularPipelines.Models;
using ModularPipelines.Options.Mac;

namespace ModularPipelines.Context.Domains.Installers;

/// <summary>
/// Provides methods for installing software on macOS systems.
/// </summary>
public interface IMacInstallerContext
{
    /// <summary>
    /// Installs software using Homebrew package manager.
    /// </summary>
    /// <param name="macBrewOptions">The options specifying the package name and installation parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="CommandResult"/> containing the result of the Homebrew installation command.</returns>
    Task<CommandResult> InstallFromBrewAsync(
        MacBrewOptions macBrewOptions,
        CancellationToken cancellationToken = default);
}
