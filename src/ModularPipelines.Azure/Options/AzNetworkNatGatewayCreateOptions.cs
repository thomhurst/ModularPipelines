using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "nat", "gateway", "create")]
public record AzNetworkNatGatewayCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--public-ip-addresses")]
    public string? PublicIpAddresses { get; set; }

    [CommandSwitch("--public-ip-prefixes")]
    public string? PublicIpPrefixes { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}