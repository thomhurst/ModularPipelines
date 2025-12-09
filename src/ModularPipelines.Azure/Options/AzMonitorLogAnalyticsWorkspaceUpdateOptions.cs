using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "log-analytics", "workspace", "update")]
public record AzMonitorLogAnalyticsWorkspaceUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--capacity-reservation-level")]
    public string? CapacityReservationLevel { get; set; }

    [CliOption("--data-collection-rule")]
    public string? DataCollectionRule { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--ingestion-access")]
    public string? IngestionAccess { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--query-access")]
    public string? QueryAccess { get; set; }

    [CliOption("--quota")]
    public string? Quota { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--retention-time")]
    public string? RetentionTime { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--user-assigned")]
    public string? UserAssigned { get; set; }
}