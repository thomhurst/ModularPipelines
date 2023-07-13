using System.Runtime.InteropServices;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Options.Linux;
using ModularPipelines.Options.Mac;
using ModularPipelines.Options.Windows;

namespace ModularPipelines.Context;

public class PredefinedInstallers : IPredefinedInstallers
{
    private readonly ICommand _command;
    private readonly IEnvironmentContext _environmentContext;
    private readonly IDownloader _downloader;

    private readonly IMacInstaller _macInstaller;
    private readonly IWindowsInstaller _windowsInstaller;
    private readonly ILinuxInstaller _linuxInstaller;

    public PredefinedInstallers(ICommand command, IEnvironmentContext environmentContext, IDownloader downloader, IMacInstaller macInstaller, IWindowsInstaller windowsInstaller, ILinuxInstaller linuxInstaller)
    {
        _command = command;
        _environmentContext = environmentContext;
        _downloader = downloader;
        _macInstaller = macInstaller;
        _windowsInstaller = windowsInstaller;
        _linuxInstaller = linuxInstaller;
    }

    public async Task<CommandResult> Chocolatey()
    {
        return await _command.ExecuteCommandLineTool(new CommandLineToolOptions("cmd")
        {
            Arguments = new[]
            {
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
            }
        });
    }

    public async Task<CommandResult> Powershell7()
    {
        var operatingSystem = _environmentContext.OperatingSystem;

        if (operatingSystem == OSPlatform.Windows)
        {
            var url = _environmentContext.Is64BitOperatingSystem
                ? "https://github.com/PowerShell/PowerShell/releases/download/v7.3.5/PowerShell-7.3.5-win-x64.msi"
                : "https://github.com/PowerShell/PowerShell/releases/download/v7.3.5/PowerShell-7.3.5-win-x86.msi";

            return await _windowsInstaller.InstallMsi(new MsiInstallerOptions(url));
        }

        if (operatingSystem == OSPlatform.OSX)
        {
            return await _macInstaller.InstallFromBrew(new MacBrewOptions("powershell"));
        }

        var linuxFile = await _downloader.DownloadFileAsync(new DownloadOptions(new Uri("https://github.com/PowerShell/PowerShell/releases/download/v7.3.5/powershell_7.3.5-1.deb_amd64.deb")));

        return await _linuxInstaller.InstallFromDpkg(new DpkgInstallOptions(linuxFile));
    }
}