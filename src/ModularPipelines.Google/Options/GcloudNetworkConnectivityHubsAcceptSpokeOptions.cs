using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "accept-spoke")]
public record GcloudNetworkConnectivityHubsAcceptSpokeOptions(
[property: CliArgument] string Hub,
[property: CliOption("--spoke")] string Spoke
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}