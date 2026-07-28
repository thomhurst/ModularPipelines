using ModularPipelines.Context.Domains.Installers;

namespace ModularPipelines.Context;

/// <summary>
/// Provides functionality for installing software packages and dependencies.
/// </summary>
public interface IInstaller
{
    /// <summary>
    /// Gets access to predefined installers for common tools and packages.
    /// </summary>
    IPredefinedInstallersContext PredefinedInstallers { get; }

    /// <summary>
    /// Gets access to file-based installation functionality.
    /// </summary>
    IFileInstaller FileInstaller { get; }

    /// <summary>
    /// Gets access to Linux-specific installation functionality.
    /// </summary>
    ILinuxInstallerContext LinuxInstaller { get; }

    /// <summary>
    /// Gets access to Windows-specific installation functionality.
    /// </summary>
    IWindowsInstallerContext WindowsInstaller { get; }

    /// <summary>
    /// Gets access to macOS-specific installation functionality.
    /// </summary>
    IMacInstallerContext MacInstaller { get; }
}