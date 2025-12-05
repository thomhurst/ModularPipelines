using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "list-spokes")]
public record GcloudNetworkConnectivityHubsListSpokesOptions(
[property: CliArgument] string Hub
) : GcloudOptions
{
    [CliOption("--spoke-locations")]
    public string[]? SpokeLocations { get; set; }

    [CliOption("--view")]
    public string? View { get; set; }
}