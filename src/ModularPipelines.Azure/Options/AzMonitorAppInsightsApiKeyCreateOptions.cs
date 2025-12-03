using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "app-insights", "api-key", "create")]
public record AzMonitorAppInsightsApiKeyCreateOptions(
[property: CliOption("--api-key")] string ApiKey,
[property: CliOption("--app")] string App,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--read-properties")]
    public string? ReadProperties { get; set; }

    [CliOption("--write-properties")]
    public string? WriteProperties { get; set; }
}