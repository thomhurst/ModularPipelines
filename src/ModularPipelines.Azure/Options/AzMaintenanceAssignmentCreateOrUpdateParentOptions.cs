using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("maintenance", "assignment", "create-or-update-parent")]
public record AzMaintenanceAssignmentCreateOrUpdateParentOptions : AzOptions
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

    [CliOption("--provider-name")]
    public string? ProviderName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-id")]
    public string? ResourceId { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--resource-parent-name")]
    public string? ResourceParentName { get; set; }

    [CliOption("--resource-parent-type")]
    public string? ResourceParentType { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}