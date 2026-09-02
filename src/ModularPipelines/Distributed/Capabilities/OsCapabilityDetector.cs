using System.Runtime.InteropServices;
using ModularPipelines.Attributes;

namespace ModularPipelines.Distributed.Capabilities;

internal static class OsCapabilityDetector
{
    public static IReadOnlyList<Capability> Detect()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return ToCapabilities(OperatingSystemConditions.Windows);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return ToCapabilities(OperatingSystemConditions.Linux);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return ToCapabilities(OperatingSystemConditions.MacOS);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
        {
            return ToCapabilities(OperatingSystemConditions.FreeBSD);
        }

        return [];
    }

    private static IReadOnlyList<Capability> ToCapabilities(string operatingSystem) =>
        [.. OperatingSystemConditions.GetWorkerCapabilities(operatingSystem).Select(static name => new Capability(name))];
}
