using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "internal-ranges", "create")]
public record GcloudNetworkConnectivityInternalRangesCreateOptions(
[property: CliArgument] string InternalRange,
[property: CliArgument] string Region,
[property: CliOption("--network")] string Network,
[property: CliOption("--ip-cidr-range")] string IpCidrRange,
[property: CliOption("--prefix-length")] string PrefixLength,
[property: CliOption("--target-cidr-range")] string[] TargetCidrRange
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--overlaps")]
    public string[]? Overlaps { get; set; }

    [CliOption("--peering")]
    public string? Peering { get; set; }

    [CliOption("--usage")]
    public string? Usage { get; set; }
}