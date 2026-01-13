using ModularPipelines.Context.Domains.Installers;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Provides platform-specific software installation capabilities.
/// </summary>
internal class InstallersContext : IInstallersContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InstallersContext"/> class.
    /// </summary>
    /// <param name="windows">The Windows installer context.</param>
    /// <param name="linux">The Linux installer context.</param>
    /// <param name="mac">The macOS installer context.</param>
    /// <param name="predefined">The predefined installers context.</param>
    public InstallersContext(
        IWindowsInstallerContext windows,
        ILinuxInstallerContext linux,
        IMacInstallerContext mac,
        IPredefinedInstallersContext predefined)
    {
        Windows = windows;
        Linux = linux;
        Mac = mac;
        Predefined = predefined;
    }

    /// <inheritdoc />
    public IWindowsInstallerContext Windows { get; }

    /// <inheritdoc />
    public ILinuxInstallerContext Linux { get; }

    /// <inheritdoc />
    public IMacInstallerContext Mac { get; }

    /// <inheritdoc />
    public IPredefinedInstallersContext Predefined { get; }
}
