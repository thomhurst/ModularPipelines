using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "app-insights", "update")]
public record AzSpringAppInsightsUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--app-insights")]
    public string? AppInsights { get; set; }

    [CliOption("--app-insights-key")]
    public string? AppInsightsKey { get; set; }

    [CliFlag("--disable")]
    public bool? Disable { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--sampling-rate")]
    public string? SamplingRate { get; set; }
}