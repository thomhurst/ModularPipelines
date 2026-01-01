using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Options.Linux;
using ModularPipelines.Options.Mac;
using ModularPipelines.Options.Windows;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

[ExcludeFromCodeCoverage]
public partial class PredefinedInstallers : IPredefinedInstallers
{
    /// <summary>
    /// Version constants for predefined installers.
    /// Update these values when new versions are released.
    /// </summary>
    private static class Versions
    {
        public const string PowerShell7 = "7.3.5";
        public const string NvmWindows = "1.1.11";
        public const string NvmLinux = "0.39.4";
    }

    private readonly ICommand _command;
    private readonly IEnvironmentContext _environmentContext;
    private readonly IDownloader _downloader;

    private readonly IMacInstaller _macInstaller;
    private readonly IWindowsInstaller _windowsInstaller;
    private readonly ILinuxInstaller _linuxInstaller;
    private readonly IBash _bash;
    private readonly IZip _zip;
    private readonly IEnvironmentVariables _environmentVariables;

    public PredefinedInstallers(ICommand command,
        IEnvironmentContext environmentContext,
        IDownloader downloader,
        IMacInstaller macInstaller,
        IWindowsInstaller windowsInstaller,
        ILinuxInstaller linuxInstaller,
        IBash bash,
        IZip zip,
        IEnvironmentVariables environmentVariables)
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
    public virtual async Task<CommandResult> Chocolatey()
    {
        return await _command.ExecuteCommandLineTool(new CommandLineToolOptions("cmd")
        {
            Arguments =
            [
                @"%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe",
                "-NoProfile",
                "-InputFormat",
                "None",
                "-ExecutionPolicy",
                "Bypass",
                "-Command",
                "[System.Net.ServicePointManager]::SecurityProtocol = 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))",
                "&&",
                "SET",
                "PATH=%PATH%;%ALLUSERSPROFILE%\\chocolatey\\bin"
            ],
        }).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async virtual Task<CommandResult> Powershell7()
    {
        var operatingSystem = _environmentContext.OperatingSystem;

        if (operatingSystem == OperatingSystemIdentifier.Windows)
        {
            var arch = _environmentContext.Is64BitOperatingSystem ? "x64" : "x86";
            var url = $"https://github.com/PowerShell/PowerShell/releases/download/v{Versions.PowerShell7}/PowerShell-{Versions.PowerShell7}-win-{arch}.msi";

            return await _windowsInstaller.InstallMsi(new MsiInstallerOptions(url)).ConfigureAwait(false);
        }

        if (operatingSystem == OperatingSystemIdentifier.MacOS)
        {
            return await _macInstaller.InstallFromBrew(new MacBrewOptions("powershell")).ConfigureAwait(false);
        }

        var linuxUrl = $"https://github.com/PowerShell/PowerShell/releases/download/v{Versions.PowerShell7}/powershell_{Versions.PowerShell7}-1.deb_amd64.deb";
        var linuxFile = await _downloader.DownloadFileAsync(new DownloadFileOptions(new Uri(linuxUrl))).ConfigureAwait(false);

        return await _linuxInstaller.InstallFromDpkg(new DpkgInstallOptions(linuxFile)).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<File?> Nvm(string? version = null)
    {
        if (OperatingSystem.IsWindows())
        {
            var nvmWindowsUrl = $"https://github.com/coreybutler/nvm-windows/releases/download/{Versions.NvmWindows}/nvm-noinstall.zip";
            var zipFile = await _downloader.DownloadFileAsync(
                new DownloadFileOptions(new Uri(nvmWindowsUrl))).ConfigureAwait(false);

            var newFolder = _zip.UnZipToFolder(zipFile, Folder.CreateTemporaryFolder());

            await newFolder.GetFile("settings.txt").WriteAsync($"""
                                                               root: {Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "nvm")}
                                                               path: C:\Program Files\nodejs
                                                               arch: 64
                                                               proxy: none
                                                               """).ConfigureAwait(false);

            var symLinkFolder = newFolder.CreateFolder("nvm_symlink").GetFolder(Guid.NewGuid().ToString("N"));

            _environmentVariables.SetEnvironmentVariable("NVM_HOME", newFolder);
            _environmentVariables.SetEnvironmentVariable("NVM_SYMLINK", symLinkFolder);
            _environmentVariables.AddToPath(newFolder);
            _environmentVariables.AddToPath(symLinkFolder);

            return newFolder.FindFile(x => x.Name == "nvm.exe");
        }

        var nvmLinuxUrl = $"https://raw.githubusercontent.com/nvm-sh/nvm/v{Versions.NvmLinux}/install.sh";
        var bashScript = await _downloader.DownloadFileAsync(
            new DownloadFileOptions(new Uri(nvmLinuxUrl))).ConfigureAwait(false);

        await _bash.FromFile(new BashFileOptions(bashScript)).ConfigureAwait(false);

        await _bash.Command(new BashCommandOptions("export NVM_DIR=\"$HOME/.nvm\"")).ConfigureAwait(false);
        await _bash.Command(new BashCommandOptions("[ -s \"$NVM_DIR/nvm.sh\" ] && \\. \"$NVM_DIR/nvm.sh\"")).ConfigureAwait(false);
        await _bash.Command(new BashCommandOptions("[ -s \"$NVM_DIR/bash_completion\" ] && \\. \"$NVM_DIR/bash_completion\"")).ConfigureAwait(false);
        await _bash.Command(new BashCommandOptions("source ~/.bashrc")).ConfigureAwait(false);

        return new File("/home/runner/.nvm");
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Node(string version = "--lts")
    {
        ValidateNodeVersion(version);

        await Nvm().ConfigureAwait(false);

        if (OperatingSystem.IsWindows())
        {
            // Windows: CliWrap handles argument escaping automatically via WithArguments()
            return await _command.ExecuteCommandLineTool(new CommandLineToolOptions("nvm")
            {
                Arguments = ["install", version],
            }).ConfigureAwait(false);
        }

        // Linux/Mac: Use shell escaping since BashCommandOptions uses string interpolation.
        // The validation regex ensures no single quotes in version, making this escaping safe.
        var escapedVersion = EscapeShellArgument(version);
        return await _bash.Command(new BashCommandOptions($"nvm install {escapedVersion}")).ConfigureAwait(false);
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

    /// <summary>
    /// Escapes a string for safe use as a shell argument.
    /// </summary>
    /// <param name="argument">The argument to escape.</param>
    /// <returns>The escaped argument wrapped in single quotes.</returns>
    private static string EscapeShellArgument(string argument)
    {
        // Single-quote the argument and escape any embedded single quotes
        // by ending the quote, adding an escaped quote, and starting a new quote
        return "'" + argument.Replace("'", "'\\''") + "'";
    }

    // Matches valid nvm version formats:
    // - Flags: --lts, --latest
    // - Aliases: node, stable, unstable, iojs, system
    // - LTS codenames: lts/*, lts/argon, lts/boron, etc.
    // - Semantic versions: 18, 18.0, 18.0.0, v18, v18.0, v18.0.0
    [GeneratedRegex(@"^(--lts|--latest|node|stable|unstable|iojs|system|lts/\*|lts/[a-z]+|v?\d+(\.\d+){0,2})$", RegexOptions.IgnoreCase)]
    private static partial Regex NodeVersionRegex();
}