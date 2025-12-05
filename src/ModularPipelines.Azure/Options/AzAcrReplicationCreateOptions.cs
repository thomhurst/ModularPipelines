using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "replication", "create")]
public record AzAcrReplicationCreateOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--region-endpoint-enabled")]
    public bool? RegionEndpointEnabled { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--zone-redundancy")]
    public string? ZoneRedundancy { get; set; }
}