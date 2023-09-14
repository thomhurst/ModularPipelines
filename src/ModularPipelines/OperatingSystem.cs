using System.Runtime.InteropServices;

namespace ModularPipelines;

public static class OperatingSystem
{
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    public static bool IsOSX => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    public static bool IsFreeBSD => RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);
    
    public static OSPlatform GetOperatingSystem()
    {
        if (IsLinux)
        {
            return OSPlatform.Linux;
        }

        if (IsWindows)
        {
            return OSPlatform.Windows;
        }

        if (IsOSX)
        {
            return OSPlatform.OSX;
        }

        if (IsFreeBSD)
        {
            return OSPlatform.FreeBSD;
        }

        return OSPlatform.Create("Unknown");
    }
}