using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("services", "vpc-peerings", "connect")]
public record GcloudServicesVpcPeeringsConnectOptions(
[property: CommandSwitch("--network")] string Network,
[property: CommandSwitch("--ranges")] string Ranges
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }
}