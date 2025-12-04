using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("maintenance", "assignment", "update")]
public record AzMaintenanceAssignmentUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

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

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--provider-name")]
    public string? ProviderName { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-id")]
    public string? ResourceId { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}