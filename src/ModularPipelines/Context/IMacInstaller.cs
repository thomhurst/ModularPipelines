using ModularPipelines.Models;
using ModularPipelines.Options.Mac;

namespace ModularPipelines.Context;

/// <summary>
/// Provides methods for installing software on macOS systems.
/// </summary>
public interface IMacInstaller
{
    /// <summary>
    /// Installs software using Homebrew package manager.
    /// </summary>
    /// <param name="macBrewOptions">The options specifying the package name and installation parameters.</param>
    /// <returns>A <see cref="CommandResult"/> containing the result of the Homebrew installation command.</returns>
    Task<CommandResult> InstallFromBrew(MacBrewOptions macBrewOptions);
}