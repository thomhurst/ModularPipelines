using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("services", "vpc-peerings", "update")]
public record GcloudServicesVpcPeeringsUpdateOptions(
[property: CommandSwitch("--network")] string Network
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--ranges")]
    public string? Ranges { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }
}