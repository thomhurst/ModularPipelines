using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("maintenance", "assignment", "create")]
public record AzMaintenanceAssignmentCreateOptions(
[property: CommandSwitch("--configuration-assignment-name")] string ConfigurationAssignmentName,
[property: CommandSwitch("--provider-name")] string ProviderName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AzOptions
{
    [CommandSwitch("--config-id")]
    public string? ConfigId { get; set; }

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

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--resource-id")]
    public string? ResourceId { get; set; }
}

