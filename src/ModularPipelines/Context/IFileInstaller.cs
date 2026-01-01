using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Provides methods for installing software from local files or web URLs.
/// </summary>
public interface IFileInstaller
{
    /// <summary>
    /// Installs software from a local installer file.
    /// </summary>
    /// <param name="options">The options specifying the installer file path and installation parameters.</param>
    /// <param name="cancellationToken">A token to cancel the installation operation.</param>
    /// <returns>A <see cref="CommandResult"/> containing the result of the installation command.</returns>
    Task<CommandResult> InstallFromFileAsync(InstallerOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads and installs software from a web URL.
    /// </summary>
    /// <param name="options">The options specifying the download URL and installation parameters.</param>
    /// <param name="cancellationToken">A token to cancel the download and installation operation.</param>
    /// <returns>A <see cref="CommandResult"/> containing the result of the installation command.</returns>
    Task<CommandResult> InstallFromWebAsync(WebInstallerOptions options,
        CancellationToken cancellationToken = default);
}