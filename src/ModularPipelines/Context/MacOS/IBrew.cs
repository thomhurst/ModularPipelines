using ModularPipelines.Models;
using ModularPipelines.Options.MacOS.Brew;

namespace ModularPipelines.Context.MacOS;

/// <summary>
/// Provides functionality for interacting with the Homebrew package manager on macOS.
/// </summary>
/// <remarks>
/// Homebrew is a popular package manager for macOS that allows installing,
/// upgrading, and managing software packages (formulae) and GUI applications (casks).
/// </remarks>
public interface IBrew
{
    /// <summary>
    /// Installs a formula or cask using Homebrew.
    /// </summary>
    /// <param name="options">The install options specifying the package to install.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Install(BrewInstallOptions options, CancellationToken token = default);

    /// <summary>
    /// Uninstalls a formula or cask using Homebrew.
    /// </summary>
    /// <param name="options">The uninstall options specifying the package to remove.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Uninstall(BrewUninstallOptions options, CancellationToken token = default);

    /// <summary>
    /// Updates Homebrew and all formulae definitions.
    /// </summary>
    /// <param name="options">Optional update configuration.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Update(BrewUpdateOptions? options = default, CancellationToken token = default);

    /// <summary>
    /// Upgrades outdated formulae and casks.
    /// </summary>
    /// <param name="options">Optional upgrade configuration.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Upgrade(BrewUpgradeOptions? options = default, CancellationToken token = default);

    /// <summary>
    /// Lists installed formulae and casks.
    /// </summary>
    /// <param name="options">Optional list configuration.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> List(BrewListOptions? options = default, CancellationToken token = default);

    /// <summary>
    /// Searches for formulae and casks matching the query.
    /// </summary>
    /// <param name="options">The search options specifying the query.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Search(BrewSearchOptions options, CancellationToken token = default);

    /// <summary>
    /// Displays information about a formula or cask.
    /// </summary>
    /// <param name="options">The info options specifying the package.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Info(BrewInfoOptions options, CancellationToken token = default);

    /// <summary>
    /// Adds a third-party repository (tap) to Homebrew.
    /// </summary>
    /// <param name="options">The tap options specifying the repository.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Tap(BrewTapOptions options, CancellationToken token = default);

    /// <summary>
    /// Removes a third-party repository (tap) from Homebrew.
    /// </summary>
    /// <param name="options">The untap options specifying the repository.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Untap(BrewUntapOptions options, CancellationToken token = default);

    /// <summary>
    /// Removes old versions of installed formulae.
    /// </summary>
    /// <param name="options">Optional cleanup configuration.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Cleanup(BrewCleanupOptions? options = default, CancellationToken token = default);

    /// <summary>
    /// Executes a custom Homebrew command with the specified options.
    /// </summary>
    /// <param name="options">The custom command options.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Custom(BrewOptions options, CancellationToken token = default);
}
