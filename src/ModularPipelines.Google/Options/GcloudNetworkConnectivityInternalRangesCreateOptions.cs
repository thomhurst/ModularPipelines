using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "internal-ranges", "create")]
public record GcloudNetworkConnectivityInternalRangesCreateOptions(
[property: PositionalArgument] string InternalRange,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--network")] string Network,
[property: CommandSwitch("--ip-cidr-range")] string IpCidrRange,
[property: CommandSwitch("--prefix-length")] string PrefixLength,
[property: CommandSwitch("--target-cidr-range")] string[] TargetCidrRange
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--overlaps")]
    public string[]? Overlaps { get; set; }

    [CommandSwitch("--peering")]
    public string? Peering { get; set; }

    [CommandSwitch("--usage")]
    public string? Usage { get; set; }
}