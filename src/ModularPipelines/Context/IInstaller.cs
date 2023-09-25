namespace ModularPipelines.Context;

public interface IInstaller
{
    IPredefinedInstallers PredefinedInstallers { get; }

    IFileInstaller FileInstaller { get; }

    ILinuxInstaller LinuxInstaller { get; }

    IWindowsInstaller WindowsInstaller { get; }

    IMacInstaller MacInstaller { get; }
}