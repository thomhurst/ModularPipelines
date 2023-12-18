using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "app-insights", "component", "create")]
public record AzMonitorAppInsightsComponentCreateOptions(
[property: CommandSwitch("--app")] string App,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--application-type")]
    public string? ApplicationType { get; set; }

    [CommandSwitch("--ingestion-access")]
    public string? IngestionAccess { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--query-access")]
    public string? QueryAccess { get; set; }

    [CommandSwitch("--retention-time")]
    public string? RetentionTime { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--workspace")]
    public string? Workspace { get; set; }
}

