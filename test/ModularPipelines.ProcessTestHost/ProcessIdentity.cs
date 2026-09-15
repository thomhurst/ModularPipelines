using System.Diagnostics;

namespace ModularPipelines.ProcessTestHost;

public readonly record struct ProcessIdentity(int Id, long StartTimeUtcTicks)
{
    public static ProcessIdentity Capture(Process process) => new(process.Id, process.StartTime.ToUniversalTime().Ticks);
}
