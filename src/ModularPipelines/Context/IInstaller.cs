namespace ModularPipelines.Context;

/// <summary>
/// Provides functionality for installing software packages and dependencies.
/// </summary>
public interface IInstaller
{
    /// <summary>
    /// Gets access to predefined installers for common tools and packages.
    /// </summary>
    IPredefinedInstallers PredefinedInstallers { get; }

    /// <summary>
    /// Gets access to file-based installation functionality.
    /// </summary>
    IFileInstaller FileInstaller { get; }

    /// <summary>
    /// Gets access to Linux-specific installation functionality.
    /// </summary>
    ILinuxInstaller LinuxInstaller { get; }

    /// <summary>
    /// Gets access to Windows-specific installation functionality.
    /// </summary>
    IWindowsInstaller WindowsInstaller { get; }

    /// <summary>
    /// Gets access to macOS-specific installation functionality.
    /// </summary>
    IMacInstaller MacInstaller { get; }
}