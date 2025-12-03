using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "log-analytics", "query-pack", "query", "create")]
public record AzMonitorLogAnalyticsQueryPackQueryCreateOptions(
[property: CliOption("--body")] string Body,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--query-pack-name")] string QueryPackName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--categories")]
    public string? Categories { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--resource-types")]
    public string? ResourceTypes { get; set; }

    [CliOption("--solutions")]
    public string? Solutions { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}