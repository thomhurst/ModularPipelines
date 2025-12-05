using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("grafana", "dashboard", "sync")]
public record AzGrafanaDashboardSyncOptions(
[property: CliOption("--destination")] string Destination,
[property: CliOption("--source")] string Source
) : AzOptions
{
    [CliOption("--dashboards-to-exclude")]
    public string? DashboardsToExclude { get; set; }

    [CliOption("--dashboards-to-include")]
    public string? DashboardsToInclude { get; set; }

    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliOption("--folders-to-exclude")]
    public string? FoldersToExclude { get; set; }

    [CliOption("--folders-to-include")]
    public string? FoldersToInclude { get; set; }
}