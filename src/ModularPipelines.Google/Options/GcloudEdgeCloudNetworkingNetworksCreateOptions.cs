using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "networks", "create")]
public record GcloudEdgeCloudNetworkingNetworksCreateOptions(
[property: CliArgument] string Network,
[property: CliArgument] string Location,
[property: CliArgument] string Zone
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--mtu")]
    public string? Mtu { get; set; }
}