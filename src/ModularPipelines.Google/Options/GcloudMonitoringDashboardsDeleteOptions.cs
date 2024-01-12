using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitoring", "dashboards", "delete")]
public record GcloudMonitoringDashboardsDeleteOptions(
[property: PositionalArgument] string Dashboard
) : GcloudOptions;