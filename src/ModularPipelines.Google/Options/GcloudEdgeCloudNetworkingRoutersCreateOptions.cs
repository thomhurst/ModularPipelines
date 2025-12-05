using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "routers", "create")]
public record GcloudEdgeCloudNetworkingRoutersCreateOptions(
[property: CliArgument] string Router,
[property: CliArgument] string Location,
[property: CliArgument] string Zone,
[property: CliOption("--asn")] string Asn,
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}