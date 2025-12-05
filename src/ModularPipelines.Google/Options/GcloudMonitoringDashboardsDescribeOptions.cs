using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitoring", "dashboards", "describe")]
public record GcloudMonitoringDashboardsDescribeOptions(
[property: CliArgument] string Dashboard
) : GcloudOptions;