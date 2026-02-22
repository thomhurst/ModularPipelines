using System.Runtime.InteropServices;

namespace ModularPipelines.Distributed.Capabilities;

internal static class OsCapabilityDetector
{
    public static string? Detect()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "windows";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return "linux";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "macos";
        }

        return null;
    }
}
