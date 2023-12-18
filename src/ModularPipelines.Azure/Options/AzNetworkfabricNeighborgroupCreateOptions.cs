using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "neighborgroup", "create")]
public record AzNetworkfabricNeighborgroupCreateOptions(
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName
) : AzOptions
{
    [CommandSwitch("--annotation")]
    public string? Annotation { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}