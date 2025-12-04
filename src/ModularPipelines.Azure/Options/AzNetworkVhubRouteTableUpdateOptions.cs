using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vhub", "route-table", "update")]
public record AzNetworkVhubRouteTableUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vhub-name")] string VhubName
) : AzOptions
{
    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}