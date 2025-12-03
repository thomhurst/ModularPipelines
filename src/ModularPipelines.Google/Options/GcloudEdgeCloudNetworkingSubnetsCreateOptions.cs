using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "subnets", "create")]
public record GcloudEdgeCloudNetworkingSubnetsCreateOptions(
[property: CliArgument] string Subnet,
[property: CliArgument] string Location,
[property: CliArgument] string Zone,
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--ipv4-range")]
    public string[]? Ipv4Range { get; set; }

    [CliOption("--ipv6-range")]
    public string[]? Ipv6Range { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--vlan-id")]
    public string? VlanId { get; set; }
}