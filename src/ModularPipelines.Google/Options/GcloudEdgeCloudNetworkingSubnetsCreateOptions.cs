using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "subnets", "create")]
public record GcloudEdgeCloudNetworkingSubnetsCreateOptions(
[property: PositionalArgument] string Subnet,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Zone,
[property: CommandSwitch("--network")] string Network
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--ipv4-range")]
    public string[]? Ipv4Range { get; set; }

    [CommandSwitch("--ipv6-range")]
    public string[]? Ipv6Range { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--vlan-id")]
    public string? VlanId { get; set; }
}