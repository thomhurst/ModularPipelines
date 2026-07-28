using ModularPipelines.Context.Domains.Installers;

namespace ModularPipelines.Context;

internal class Installer : IInstaller
{
    public IPredefinedInstallersContext PredefinedInstallers { get; }

    public IFileInstaller FileInstaller { get; }

    public ILinuxInstallerContext LinuxInstaller { get; }

    public IWindowsInstallerContext WindowsInstaller { get; }

    public IMacInstallerContext MacInstaller { get; }

    public Installer(IPredefinedInstallersContext predefinedInstallers, IFileInstaller fileInstaller, ILinuxInstallerContext linuxInstaller, IWindowsInstallerContext windowsInstaller, IMacInstallerContext macInstaller)
    {
        PredefinedInstallers = predefinedInstallers;
        FileInstaller = fileInstaller;
        LinuxInstaller = linuxInstaller;
        WindowsInstaller = windowsInstaller;
        MacInstaller = macInstaller;
    }
}