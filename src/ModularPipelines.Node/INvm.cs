using ModularPipelines.Models;

namespace ModularPipelines.Node;

/// <summary>
/// Runs nvm-sh on Linux/macOS or nvm-windows on Windows.
/// </summary>
/// <remarks>
/// Unix calls require Bash and source nvm.sh from NVM_DIR, or from
/// XDG_CONFIG_HOME/nvm (when set), otherwise HOME/.nvm. Each call runs in a separate
/// child shell and cannot change the pipeline process's environment.
/// </remarks>
public interface INvm
{
    /// <summary>
    /// Selects a Node.js version. On Unix the selection ends with this call's child shell;
    /// subsequent Node/npm commands do not inherit it. On Windows nvm changes its shared
    /// Node.js symlink, which may require elevation and affects other processes.
    /// </summary>
    Task<CommandResult> UseAsync(string version, CancellationToken cancellationToken = default);

    /// <summary>
    /// Installs a Node.js version. Any Unix environment changes stay in the child shell.
    /// </summary>
    Task<CommandResult> InstallAsync(string version, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reports the version of nvm itself: nvm --version on Unix, nvm version on Windows.
    /// </summary>
    Task<CommandResult> VersionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Reports the active Node.js version with nvm current. On Unix this reflects the
    /// inherited PATH; no default version is automatically selected when loading nvm.
    /// </summary>
    Task<CommandResult> CurrentAsync(CancellationToken cancellationToken = default);
}
