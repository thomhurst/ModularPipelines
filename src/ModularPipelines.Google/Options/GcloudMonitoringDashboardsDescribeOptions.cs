using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitoring", "dashboards", "describe")]
public record GcloudMonitoringDashboardsDescribeOptions(
[property: PositionalArgument] string Dashboard
) : GcloudOptions;