using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("maintenance", "assignment", "create")]
public record AzMaintenanceAssignmentCreateOptions(
[property: CliOption("--configuration-assignment-name")] string ConfigurationAssignmentName,
[property: CliOption("--provider-name")] string ProviderName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--resource-type")] string ResourceType
) : AzOptions
{
    [CliOption("--config-id")]
    public string? ConfigId { get; set; }

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

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--resource-id")]
    public string? ResourceId { get; set; }
}