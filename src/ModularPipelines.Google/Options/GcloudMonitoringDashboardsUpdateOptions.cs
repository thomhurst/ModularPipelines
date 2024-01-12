using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitoring", "dashboards", "update")]
public record GcloudMonitoringDashboardsUpdateOptions(
[property: PositionalArgument] string Dashboard,
[property: CommandSwitch("--config")] string Config,
[property: CommandSwitch("--config-from-file")] string ConfigFromFile
) : GcloudOptions;