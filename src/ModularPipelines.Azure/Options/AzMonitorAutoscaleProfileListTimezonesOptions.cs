using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "autoscale", "profile", "list-timezones")]
public record AzMonitorAutoscaleProfileListTimezonesOptions : AzOptions
{
    [CliOption("--offset")]
    public string? Offset { get; set; }

    [CliOption("--search-query")]
    public string? SearchQuery { get; set; }
}