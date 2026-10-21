using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Provides generic software installation capabilities.
/// </summary>
public interface IInstallersContext
{
    /// <summary>
    /// Runs an installer from the local file system.
    /// </summary>
    /// <remarks>
    /// Windows MSI packages are run with <c>msiexec /i</c>. PowerShell scripts are run with
    /// <c>pwsh -File</c>, which requires PowerShell to be installed. Other files are executed directly.
    /// </remarks>
    /// <param name="options">The installer options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<CommandResult> InstallAsync(
        InstallerOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads and runs an installer.
    /// </summary>
    /// <remarks>
    /// The downloaded file's extension is inferred from the response filename or final redirect URI.
    /// The file is deleted after installation, including when installation fails or is cancelled.
    /// </remarks>
    /// <param name="options">The web installer options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<CommandResult> InstallFromWebAsync(
        WebInstallerOptions options,
        CancellationToken cancellationToken = default);
}
