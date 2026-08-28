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
    /// <param name="options">The installer options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<CommandResult> InstallAsync(
        InstallerOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads and runs an installer.
    /// </summary>
    /// <param name="options">The web installer options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<CommandResult> InstallFromWebAsync(
        WebInstallerOptions options,
        CancellationToken cancellationToken = default);
}
