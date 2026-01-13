using ModularPipelines.Models;
using ModularPipelines.Options.Windows.Winget;

namespace ModularPipelines.Context.Windows;

/// <summary>
/// Provides functionality for interacting with the Windows Package Manager (winget).
/// </summary>
/// <remarks>
/// Windows Package Manager (winget) is the official command-line package manager
/// for Windows 10 (1709+) and Windows 11, allowing installation and management
/// of applications from the Microsoft Store and other sources.
/// </remarks>
public interface IWinget
{
    /// <summary>
    /// Installs a package using winget.
    /// </summary>
    /// <param name="options">The install options specifying the package to install.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Install(WingetInstallOptions options, CancellationToken token = default);

    /// <summary>
    /// Uninstalls a package using winget.
    /// </summary>
    /// <param name="options">The uninstall options specifying the package to remove.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Uninstall(WingetUninstallOptions options, CancellationToken token = default);

    /// <summary>
    /// Upgrades a package or all packages using winget.
    /// </summary>
    /// <param name="options">The upgrade options.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Upgrade(WingetUpgradeOptions? options = default, CancellationToken token = default);

    /// <summary>
    /// Lists installed packages.
    /// </summary>
    /// <param name="options">Optional list configuration.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> List(WingetListOptions? options = default, CancellationToken token = default);

    /// <summary>
    /// Searches for packages in the winget repository.
    /// </summary>
    /// <param name="options">The search options specifying the query.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Search(WingetSearchOptions options, CancellationToken token = default);

    /// <summary>
    /// Shows detailed information about a package.
    /// </summary>
    /// <param name="options">The show options specifying the package.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Show(WingetShowOptions options, CancellationToken token = default);

    /// <summary>
    /// Exports a list of installed packages.
    /// </summary>
    /// <param name="options">The export options.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Export(WingetExportOptions options, CancellationToken token = default);

    /// <summary>
    /// Imports packages from a file.
    /// </summary>
    /// <param name="options">The import options.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Import(WingetImportOptions options, CancellationToken token = default);

    /// <summary>
    /// Manages winget sources.
    /// </summary>
    /// <param name="options">The source options.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Source(WingetSourceOptions options, CancellationToken token = default);

    /// <summary>
    /// Executes a custom winget command with the specified options.
    /// </summary>
    /// <param name="options">The custom command options.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The command result containing output and exit code.</returns>
    Task<CommandResult> Custom(WingetOptions options, CancellationToken token = default);
}
