using System.Runtime.InteropServices;
using ModularPipelines.Attributes;

namespace ModularPipelines.Distributed.Capabilities;

internal static class OsCapabilityDetector
{
    public static IReadOnlyList<string> Detect()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.Windows);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.Linux);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.MacOS);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
        {
            return OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.FreeBSD);
        }

        return [];
    }
}
