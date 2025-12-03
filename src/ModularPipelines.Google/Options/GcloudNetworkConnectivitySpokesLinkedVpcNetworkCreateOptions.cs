using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "spokes", "linked-vpc-network", "create")]
public record GcloudNetworkConnectivitySpokesLinkedVpcNetworkCreateOptions(
[property: CliArgument] string Spoke,
[property: CliOption("--hub")] string Hub,
[property: CliOption("--vpc-network")] string VpcNetwork
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--exclude-export-ranges")]
    public string[]? ExcludeExportRanges { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}