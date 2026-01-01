using ModularPipelines.Models;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

/// <summary>
/// Provides methods for installing common development tools and packages.
/// </summary>
public interface IPredefinedInstallers
{
    /// <summary>
    /// Installs the Chocolatey package manager for Windows.
    /// </summary>
    /// <returns>The result of the installation command.</returns>
    Task<CommandResult> Chocolatey();

    /// <summary>
    /// Installs PowerShell 7 on the current system.
    /// </summary>
    /// <returns>The result of the installation command.</returns>
    /// <remarks>
    /// On Windows, downloads and installs the MSI package.
    /// On macOS, installs via Homebrew.
    /// On Linux, installs the deb package.
    /// </remarks>
    Task<CommandResult> Powershell7();

    /// <summary>
    /// Installs Node Version Manager (nvm).
    /// </summary>
    /// <param name="version">Optional specific version to install.</param>
    /// <returns>The nvm executable file, or null if installation failed.</returns>
    Task<File?> Nvm(string? version = null);

    /// <summary>
    /// Installs Node.js using nvm.
    /// </summary>
    /// <param name="version">
    /// The Node.js version to install. Defaults to "--lts" for the latest LTS version.
    /// Supports formats like "18.0.0", "v18", "--lts", "--latest", "lts/argon", etc.
    /// </param>
    /// <returns>The result of the installation command.</returns>
    Task<CommandResult> Node(string version = "--lts");
}