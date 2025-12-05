using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "configuration", "show-metric")]
public record AzIotHubConfigurationShowMetricOptions(
[property: CliOption("--config-id")] string ConfigId,
[property: CliOption("--metric-id")] string MetricId
) : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--metric-type")]
    public string? MetricType { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}