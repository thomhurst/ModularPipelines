using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "hubs", "list-spokes")]
public record GcloudNetworkConnectivityHubsListSpokesOptions(
[property: PositionalArgument] string Hub
) : GcloudOptions
{
    [CommandSwitch("--spoke-locations")]
    public string[]? SpokeLocations { get; set; }

    [CommandSwitch("--view")]
    public string? View { get; set; }
}