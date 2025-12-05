using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitoring", "dashboards", "delete")]
public record GcloudMonitoringDashboardsDeleteOptions(
[property: CliArgument] string Dashboard
) : GcloudOptions;