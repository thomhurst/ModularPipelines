namespace ModularPipelines.Context;

internal class Installer : IInstaller
{
    public IPredefinedInstallers PredefinedInstallers { get; }

    public IFileInstaller FileInstaller { get; }

    public ILinuxInstaller LinuxInstaller { get; }

    public IWindowsInstaller WindowsInstaller { get; }

    public IMacInstaller MacInstaller { get; }

    public Installer(IPredefinedInstallers predefinedInstallers, IFileInstaller fileInstaller, ILinuxInstaller linuxInstaller, IWindowsInstaller windowsInstaller, IMacInstaller macInstaller)
    {
        PredefinedInstallers = predefinedInstallers;
        FileInstaller = fileInstaller;
        LinuxInstaller = linuxInstaller;
        WindowsInstaller = windowsInstaller;
        MacInstaller = macInstaller;
    }
}