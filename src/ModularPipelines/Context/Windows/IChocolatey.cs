using ModularPipelines.Models;
using ModularPipelines.Options.Windows.Chocolatey;

namespace ModularPipelines.Context.Windows;

/// <summary>
/// Provides functionality for interacting with the Chocolatey package manager on Windows.
/// </summary>
/// <remarks>
/// Chocolatey is a popular package manager for Windows that automates the process of
/// installing, upgrading, configuring, and removing software.
/// </remarks>
public interface IChocolatey
{
    /// <summary>
    /// Installs a package using Chocolatey.
    /// </summary>
    /// <param name="options">The install options specifying the package to install.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Install(ChocolateyInstallOptions options, CancellationToken token = default);

    /// <summary>
    /// Uninstalls a package using Chocolatey.
    /// </summary>
    /// <param name="options">The uninstall options specifying the package to remove.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Uninstall(ChocolateyUninstallOptions options, CancellationToken token = default);

    /// <summary>
    /// Upgrades a package or all packages using Chocolatey.
    /// </summary>
    /// <param name="options">The upgrade options.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Upgrade(ChocolateyUpgradeOptions options, CancellationToken token = default);

    /// <summary>
    /// Lists installed packages.
    /// </summary>
    /// <param name="options">Optional list configuration.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> List(ChocolateyListOptions? options = default, CancellationToken token = default);

    /// <summary>
    /// Searches for packages in the Chocolatey repository.
    /// </summary>
    /// <param name="options">The search options specifying the query.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Search(ChocolateySearchOptions options, CancellationToken token = default);

    /// <summary>
    /// Displays information about a package.
    /// </summary>
    /// <param name="options">The info options specifying the package.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Info(ChocolateyInfoOptions options, CancellationToken token = default);

    /// <summary>
    /// Lists outdated packages.
    /// </summary>
    /// <param name="options">Optional configuration.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Outdated(ChocolateyOutdatedOptions? options = default, CancellationToken token = default);

    /// <summary>
    /// Pins a package to prevent it from being upgraded.
    /// </summary>
    /// <param name="options">The pin options.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Pin(ChocolateyPinOptions options, CancellationToken token = default);

    /// <summary>
    /// Executes a custom Chocolatey command with the specified options.
    /// </summary>
    /// <param name="options">The custom command options.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Custom(ChocolateyOptions options, CancellationToken token = default);
}
