using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudMonitoring
{
    public GcloudMonitoring(
        GcloudMonitoringDashboards dashboards,
        GcloudMonitoringSnoozes snoozes
    )
    {
        Dashboards = dashboards;
        Snoozes = snoozes;
    }

    public GcloudMonitoringDashboards Dashboards { get; }

    public GcloudMonitoringSnoozes Snoozes { get; }
}