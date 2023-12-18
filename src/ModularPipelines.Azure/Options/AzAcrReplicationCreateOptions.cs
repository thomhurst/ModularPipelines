using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "replication", "create")]
public record AzAcrReplicationCreateOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--region-endpoint-enabled")]
    public bool? RegionEndpointEnabled { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--zone-redundancy")]
    public string? ZoneRedundancy { get; set; }
}