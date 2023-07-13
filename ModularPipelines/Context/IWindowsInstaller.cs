using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Options.Windows;

namespace ModularPipelines.Context;

public interface IWindowsInstaller
{
    Task<CommandResult> InstallMsi(MsiInstallerOptions msiInstallerOptions);
    Task<CommandResult> InstallExe(ExeInstallerOptions exeInstallerOptions);
}