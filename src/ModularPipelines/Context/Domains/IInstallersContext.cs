using ModularPipelines.Context.Domains.Installers;

#pragma warning disable SA1135 // Using directives should be qualified
using ModularPipelines.Context;
#pragma warning restore SA1135 // Using directives should be qualified

namespace ModularPipelines.Context.Domains;

/// <summary>
/// Provides platform-specific software installation capabilities.
/// </summary>
public interface IInstallersContext
{
    /// <summary>
    /// File-based installers (local files and web downloads).
    /// </summary>
    IFileInstaller File { get; }

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
