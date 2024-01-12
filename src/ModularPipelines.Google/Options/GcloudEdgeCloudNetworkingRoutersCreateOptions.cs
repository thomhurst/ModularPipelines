using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "routers", "create")]
public record GcloudEdgeCloudNetworkingRoutersCreateOptions(
[property: PositionalArgument] string Router,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Zone,
[property: CommandSwitch("--asn")] string Asn,
[property: CommandSwitch("--network")] string Network
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}