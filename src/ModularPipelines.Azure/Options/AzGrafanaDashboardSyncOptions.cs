using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "dashboard", "sync")]
public record AzGrafanaDashboardSyncOptions(
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--source")] string Source
) : AzOptions
{
    [CommandSwitch("--dashboards-to-exclude")]
    public string? DashboardsToExclude { get; set; }

    [CommandSwitch("--dashboards-to-include")]
    public string? DashboardsToInclude { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [CommandSwitch("--folders-to-exclude")]
    public string? FoldersToExclude { get; set; }

    [CommandSwitch("--folders-to-include")]
    public string? FoldersToInclude { get; set; }
}

