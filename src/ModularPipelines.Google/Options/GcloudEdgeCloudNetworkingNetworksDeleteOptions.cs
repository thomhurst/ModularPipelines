using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "networks", "delete")]
public record GcloudEdgeCloudNetworkingNetworksDeleteOptions(
[property: CliArgument] string Network,
[property: CliArgument] string Location,
[property: CliArgument] string Zone
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}