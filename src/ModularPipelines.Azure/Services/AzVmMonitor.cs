using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm")]
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