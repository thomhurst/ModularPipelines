using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "reject-spoke")]
public record GcloudNetworkConnectivityHubsRejectSpokeOptions(
[property: CliArgument] string Hub,
[property: CliOption("--spoke")] string Spoke
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--details")]
    public string? Details { get; set; }
}