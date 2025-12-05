using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitoring", "dashboards", "create")]
public record GcloudMonitoringDashboardsCreateOptions(
[property: CliOption("--config")] string Config,
[property: CliOption("--config-from-file")] string ConfigFromFile
) : GcloudOptions
{
    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }
}