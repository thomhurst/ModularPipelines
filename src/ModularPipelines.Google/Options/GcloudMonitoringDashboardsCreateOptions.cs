using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitoring", "dashboards", "create")]
public record GcloudMonitoringDashboardsCreateOptions(
[property: CommandSwitch("--config")] string Config,
[property: CommandSwitch("--config-from-file")] string ConfigFromFile
) : GcloudOptions
{
    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }
}