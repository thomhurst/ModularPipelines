using ModularPipelines.Context.Domains.Installers;

namespace ModularPipelines.Context.Domains;

/// <summary>
/// Provides platform-specific software installation capabilities.
/// </summary>
public interface IInstallersContext
{
    /// <summary>
    /// Windows installers (MSI, EXE, Chocolatey).
    /// </summary>
    IWindowsInstallerContext Windows { get; }

    /// <summary>
    /// Linux installers (apt, dpkg).
    /// </summary>
    ILinuxInstallerContext Linux { get; }

    /// <summary>
    /// macOS installers (Homebrew).
    /// </summary>
    IMacInstallerContext Mac { get; }

    /// <summary>
    /// Pre-configured installers for common tools.
    /// </summary>
    IPredefinedInstallersContext Predefined { get; }
}
