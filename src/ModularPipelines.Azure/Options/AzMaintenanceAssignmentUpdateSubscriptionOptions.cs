using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("maintenance", "assignment", "update-subscription")]
public record AzMaintenanceAssignmentUpdateSubscriptionOptions : AzOptions
{
    [CliOption("--config-id")]
    public string? ConfigId { get; set; }

    [CliOption("--configuration-assignment-name")]
    public string? ConfigurationAssignmentName { get; set; }

    [CliOption("--filter-locations")]
    public string? FilterLocations { get; set; }

    [CliOption("--filter-os-types")]
    public string? FilterOsTypes { get; set; }

    [CliOption("--filter-resource-groups")]
    public string? FilterResourceGroups { get; set; }

    [CliOption("--filter-resource-types")]
    public string? FilterResourceTypes { get; set; }

    [CliOption("--filter-tags")]
    public string? FilterTags { get; set; }

    [CliOption("--filter-tags-operator")]
    public string? FilterTagsOperator { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--resource-id")]
    public string? ResourceId { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}