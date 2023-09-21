namespace ModularPipelines;

public static class OperatingSystemHelper
{
    public static OperatingSystemIdentifier GetOperatingSystem()
    {
        if (OperatingSystem.IsLinux())
        {
            return OperatingSystemIdentifier.Linux;
        }

        if (OperatingSystem.IsWindows())       
        {
            return OperatingSystemIdentifier.Windows;
        }

        if (OperatingSystem.IsMacOS())
        {
            return OperatingSystemIdentifier.MacOS;
        }

        if (OperatingSystem.IsFreeBSD())
        {
            return OperatingSystemIdentifier.FreeBSD;
        }

        return OperatingSystemIdentifier.Unknown;
    }
}