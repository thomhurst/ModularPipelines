using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitoring", "dashboards", "list")]
public record GcloudMonitoringDashboardsListOptions : GcloudOptions;