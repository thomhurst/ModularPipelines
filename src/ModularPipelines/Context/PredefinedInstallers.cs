using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using ModularPipelines.Context.Domains;
using ModularPipelines.Context.Domains.Environment;
using ModularPipelines.Context.Domains.Files;
using ModularPipelines.Context.Domains.Installers;
using ModularPipelines.Context.Domains.Network;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.FileSystem;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Options.Linux;
using ModularPipelines.Options.Mac;
using ModularPipelines.Options.Windows;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

/// <summary>
/// Provides predefined installers for common development tools.
/// </summary>
[ExcludeFromCodeCoverage]
public partial class PredefinedInstallers : IPredefinedInstallersContext
{
    /// <summary>
    /// Version constants for predefined installers.
    /// These are default versions that may become outdated over time.
    /// Update these values when new versions are released.
    /// </summary>
    /// <remarks>
    /// These versions are used to construct download URLs for various installers.
    /// Check the respective project repositories for the latest available versions:
    /// - PowerShell: https://github.com/PowerShell/PowerShell/releases.
    /// - NVM for Windows: https://github.com/coreybutler/nvm-windows/releases.
    /// - NVM for Unix: https://github.com/nvm-sh/nvm/releases.
    /// </remarks>
    private static class Versions
    {
        /// <summary>
        /// Default PowerShell 7 version for installers.
        /// This version may become outdated; check https://github.com/PowerShell/PowerShell/releases for newer versions.
        /// </summary>
        public const string PowerShell7 = "7.3.5";

        /// <summary>
        /// Default NVM for Windows version.
        /// This version may become outdated; check https://github.com/coreybutler/nvm-windows/releases for newer versions.
        /// </summary>
        public const string NvmWindows = "1.1.11";

        /// <summary>
        /// Default NVM for Unix (Linux/macOS) version.
        /// This version may become outdated; check https://github.com/nvm-sh/nvm/releases for newer versions.
        /// </summary>
        public const string NvmLinux = "0.39.4";
    }

    private readonly ICommandContext _command;
    private readonly IEnvironmentContext _environmentContext;
    private readonly IDownloaderContext _downloader;

    private readonly IMacInstallerContext _macInstaller;
    private readonly IWindowsInstallerContext _windowsInstaller;
    private readonly ILinuxInstallerContext _linuxInstaller;
    private readonly IBashContext _bash;
    private readonly IZipContext _zip;
    private readonly IEnvironmentVariablesContext _environmentVariables;

    public PredefinedInstallers(ICommandContext command,
        IEnvironmentContext environmentContext,
        IDownloaderContext downloader,
        IMacInstallerContext macInstaller,
        IWindowsInstallerContext windowsInstaller,
        ILinuxInstallerContext linuxInstaller,
        IBashContext bash,
        IZipContext zip,
        IEnvironmentVariablesContext environmentVariables)
    {
        _command = command;
        _environmentContext = environmentContext;
        _downloader = downloader;
        _macInstaller = macInstaller;
        _windowsInstaller = windowsInstaller;
        _linuxInstaller = linuxInstaller;
        _bash = bash;
        _zip = zip;
        _environmentVariables = environmentVariables;
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ChocolateyAsync(CancellationToken cancellationToken = default)
    {
        var result = await _command.ExecuteCommandLineToolAsync(new GenericCommandLineToolOptions("powershell.exe")
        {
            Arguments =
            [
                "-NoProfile",
                "-InputFormat",
                "None",
                "-ExecutionPolicy",
                "Bypass",
                "-Command",
                "[System.Net.ServicePointManager]::SecurityProtocol = 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))",
            ],
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

        var allUsersProfile = _environmentVariables.Get("ALLUSERSPROFILE")
                              ?? Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        _environmentVariables.AddToPath(Path.Combine(allUsersProfile, "chocolatey", "bin"));

        return result;
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Powershell7Async(CancellationToken cancellationToken = default)
    {
        var operatingSystem = _environmentContext.OperatingSystem;

        if (operatingSystem == OSPlatform.Windows)
        {
            var arch = _environmentContext.Architecture == Architecture.X86 ? "x86" : "x64";
            var url = $"https://github.com/PowerShell/PowerShell/releases/download/v{Versions.PowerShell7}/PowerShell-{Versions.PowerShell7}-win-{arch}.msi";

            return await _windowsInstaller.InstallMsiAsync(new MsiInstallerOptions(url), cancellationToken).ConfigureAwait(false);
        }

        if (operatingSystem == OSPlatform.OSX)
        {
            return await _macInstaller.InstallFromBrewAsync(new MacBrewOptions("powershell"), cancellationToken).ConfigureAwait(false);
        }

        var linuxUrl = $"https://github.com/PowerShell/PowerShell/releases/download/v{Versions.PowerShell7}/powershell_{Versions.PowerShell7}-1.deb_amd64.deb";
        var linuxFile = await _downloader.DownloadFileAsync(
            new DownloadFileOptions(new Uri(linuxUrl)),
            cancellationToken).ConfigureAwait(false);

        return await _linuxInstaller.InstallFromDpkgAsync(new DpkgInstallOptions(linuxFile), cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<File?> NvmAsync(string? version = null, CancellationToken cancellationToken = default)
    {
        if (_environmentContext.OperatingSystem == OSPlatform.Windows)
        {
            var nvmWindowsUrl = $"https://github.com/coreybutler/nvm-windows/releases/download/{Versions.NvmWindows}/nvm-noinstall.zip";
            var zipFile = await _downloader.DownloadFileAsync(
                new DownloadFileOptions(new Uri(nvmWindowsUrl)),
                cancellationToken).ConfigureAwait(false);

            var newFolder = _zip.UnZipToFolder(zipFile, Folder.CreateTemporaryFolder());

            var nvmRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "nvm");
            var nodejsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "nodejs");

            await newFolder.GetFile("settings.txt").WriteAsync($"""
                                                               root: {nvmRoot}
                                                               path: {nodejsPath}
                                                               arch: 64
                                                               proxy: none
                                                               """, cancellationToken).ConfigureAwait(false);

            var symLinkFolder = newFolder.CreateFolder("nvm_symlink").GetFolder(Guid.NewGuid().ToString("N"));

            _environmentVariables.Set("NVM_HOME", newFolder);
            _environmentVariables.Set("NVM_SYMLINK", symLinkFolder);
            _environmentVariables.AddToPath(newFolder);
            _environmentVariables.AddToPath(symLinkFolder);

            return newFolder.FindFile(x => x.Name == "nvm.exe");
        }

        var nvmLinuxUrl = $"https://raw.githubusercontent.com/nvm-sh/nvm/v{Versions.NvmLinux}/install.sh";
        var bashScript = await _downloader.DownloadFileAsync(
            new DownloadFileOptions(new Uri(nvmLinuxUrl)),
            cancellationToken).ConfigureAwait(false);

        await _bash.FromFileAsync(new BashFileOptions(bashScript), cancellationToken).ConfigureAwait(false);

        var nvmDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nvm");
        return new File(nvmDir);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> NodeAsync(
        string version = "--lts",
        CancellationToken cancellationToken = default)
    {
        ValidateNodeVersion(version);

        await NvmAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        if (_environmentContext.OperatingSystem == OSPlatform.Windows)
        {
            // Windows: CliWrap handles argument escaping automatically via WithArguments()
            return await _command.ExecuteCommandLineToolAsync(new GenericCommandLineToolOptions("nvm")
            {
                Arguments = ["install", version],
            }, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        // Linux/Mac: Use shell escaping since BashCommandOptions uses string interpolation.
        var escapedVersion = ShellArgumentEscaper.Escape(version);
        return await _bash.CommandAsync(new BashCommandOptions(
            $"export NVM_DIR=\"$HOME/.nvm\" && [ -s \"$NVM_DIR/nvm.sh\" ] && . \"$NVM_DIR/nvm.sh\" && nvm install {escapedVersion}"),
            cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Validates that the Node.js version string is safe and well-formed.
    /// </summary>
    /// <param name="version">The version string to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the version format is invalid.</exception>
    private static void ValidateNodeVersion(string version)
    {
        ArgumentNullException.ThrowIfNull(version);

        // Allow: --lts, --latest, semantic versions (with optional 'v' prefix),
        // aliases like "node", "lts/*", "lts/argon", etc.
        if (!NodeVersionRegex().IsMatch(version))
        {
            throw new ArgumentException(
                $"Invalid Node.js version format: '{version}'. " +
                "Expected formats: --lts, --latest, v18.0.0, 18.0.0, lts/*, node, system, or similar.",
                nameof(version));
        }
    }

    // Matches valid nvm version formats:
    // - Flags: --lts, --latest
    // - Aliases: node, stable, unstable, iojs, system
    // - LTS codenames: lts/*, lts/argon, lts/boron, etc.
    // - Semantic versions: 18, 18.0, 18.0.0, v18, v18.0, v18.0.0
    [GeneratedRegex(@"^(--lts|--latest|node|stable|unstable|iojs|system|lts/\*|lts/[a-z]+|v?\d+(\.\d+){0,2})$", RegexOptions.IgnoreCase)]
    private static partial Regex NodeVersionRegex();
}
