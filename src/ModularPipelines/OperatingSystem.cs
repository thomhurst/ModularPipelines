using System.Runtime.InteropServices;

namespace ModularPipelines;

public class OperatingSystem
{
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    public static bool IsOSX => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    public static bool IsFreeBSD => RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);
}