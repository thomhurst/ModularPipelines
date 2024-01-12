using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "hubs", "accept-spoke")]
public record GcloudNetworkConnectivityHubsAcceptSpokeOptions(
[property: PositionalArgument] string Hub,
[property: CommandSwitch("--spoke")] string Spoke
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}