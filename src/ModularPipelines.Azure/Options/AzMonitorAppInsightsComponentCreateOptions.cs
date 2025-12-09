using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "app-insights", "component", "create")]
public record AzMonitorAppInsightsComponentCreateOptions(
[property: CliOption("--app")] string App,
[property: CliOption("--location")] string Location,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--application-type")]
    public string? ApplicationType { get; set; }

    [CliOption("--ingestion-access")]
    public string? IngestionAccess { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--query-access")]
    public string? QueryAccess { get; set; }

    [CliOption("--retention-time")]
    public string? RetentionTime { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}