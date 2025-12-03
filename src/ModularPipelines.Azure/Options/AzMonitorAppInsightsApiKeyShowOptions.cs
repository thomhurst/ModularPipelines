using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "app-insights", "api-key", "show")]
public record AzMonitorAppInsightsApiKeyShowOptions(
[property: CliOption("--app")] string App,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--api-key")]
    public string? ApiKey { get; set; }
}