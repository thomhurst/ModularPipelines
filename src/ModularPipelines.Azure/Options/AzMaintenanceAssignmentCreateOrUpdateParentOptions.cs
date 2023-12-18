using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("maintenance", "assignment", "create-or-update-parent")]
public record AzMaintenanceAssignmentCreateOrUpdateParentOptions(
[property: CommandSwitch("--provider-name")] string ProviderName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AzOptions
{
    [CommandSwitch("--config-id")]
    public string? ConfigId { get; set; }

    [CommandSwitch("--configuration-assignment-name")]
    public string? ConfigurationAssignmentName { get; set; }

    [CommandSwitch("--filter-locations")]
    public string? FilterLocations { get; set; }

    [CommandSwitch("--filter-os-types")]
    public string? FilterOsTypes { get; set; }

    [CommandSwitch("--filter-resource-groups")]
    public string? FilterResourceGroups { get; set; }

    [CommandSwitch("--filter-resource-types")]
    public string? FilterResourceTypes { get; set; }

    [CommandSwitch("--filter-tags")]
    public string? FilterTags { get; set; }

    [CommandSwitch("--filter-tags-operator")]
    public string? FilterTagsOperator { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--resource-id")]
    public string? ResourceId { get; set; }

    [CommandSwitch("--resource-parent-name")]
    public string? ResourceParentName { get; set; }

    [CommandSwitch("--resource-parent-type")]
    public string? ResourceParentType { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}