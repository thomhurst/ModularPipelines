using System.Diagnostics.CodeAnalysis;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Options.Linux;
using ModularPipelines.Options.Mac;
using ModularPipelines.Options.Windows;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

[ExcludeFromCodeCoverage]
public class PredefinedInstallers : IPredefinedInstallers
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
        await Nvm().ConfigureAwait(false);

        if (OperatingSystem.IsWindows())
        {
            return await _command.ExecuteCommandLineTool(new CommandLineToolOptions("nvm")
            {
                Arguments = ["install", version],
            }).ConfigureAwait(false);
        }

        return await _bash.Command(new BashCommandOptions($"nvm install {version}")).ConfigureAwait(false);
    }
}