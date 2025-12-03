using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitoring", "dashboards", "update")]
public record GcloudMonitoringDashboardsUpdateOptions(
[property: CliArgument] string Dashboard,
[property: CliOption("--config")] string Config,
[property: CliOption("--config-from-file")] string ConfigFromFile
) : GcloudOptions;