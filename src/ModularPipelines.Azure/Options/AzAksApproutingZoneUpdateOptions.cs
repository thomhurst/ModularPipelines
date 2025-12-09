using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "approuting", "zone", "update")]
public record AzAksApproutingZoneUpdateOptions(
[property: CliOption("--ids")] string Ids,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--attach-zones")]
    public bool? AttachZones { get; set; }
}