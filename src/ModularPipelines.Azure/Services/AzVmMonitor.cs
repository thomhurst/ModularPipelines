using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm")]
public class AzVmMonitor
{
    public AzVmMonitor(
        AzVmMonitorLog log,
        AzVmMonitorMetrics metrics
    )
    {
        Log = log;
        Metrics = metrics;
    }

    public AzVmMonitorLog Log { get; }

    public AzVmMonitorMetrics Metrics { get; }
}