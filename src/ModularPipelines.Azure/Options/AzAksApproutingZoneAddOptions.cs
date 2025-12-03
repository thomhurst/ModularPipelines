using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "approuting", "zone", "add")]
public record AzAksApproutingZoneAddOptions(
[property: CliOption("--ids")] string Ids,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--attach-zones")]
    public bool? AttachZones { get; set; }
}