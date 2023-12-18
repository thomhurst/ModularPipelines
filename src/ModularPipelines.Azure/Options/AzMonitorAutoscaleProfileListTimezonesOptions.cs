using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "autoscale", "profile", "list-timezones")]
public record AzMonitorAutoscaleProfileListTimezonesOptions : AzOptions
{
    [CommandSwitch("--offset")]
    public string? Offset { get; set; }

    [CommandSwitch("--search-query")]
    public string? SearchQuery { get; set; }
}