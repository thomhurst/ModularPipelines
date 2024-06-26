using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("maintenance", "assignment", "update")]
public record AzMaintenanceAssignmentUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

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

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--provider-name")]
    public string? ProviderName { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-id")]
    public string? ResourceId { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}