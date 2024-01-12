using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "hubs", "reject-spoke")]
public record GcloudNetworkConnectivityHubsRejectSpokeOptions(
[property: PositionalArgument] string Hub,
[property: CommandSwitch("--spoke")] string Spoke
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--details")]
    public string? Details { get; set; }
}