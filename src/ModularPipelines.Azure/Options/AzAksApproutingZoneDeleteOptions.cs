using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "approuting", "zone", "delete")]
public record AzAksApproutingZoneDeleteOptions(
[property: CliOption("--ids")] string Ids,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}