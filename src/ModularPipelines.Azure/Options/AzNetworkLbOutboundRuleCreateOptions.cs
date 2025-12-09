using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "lb", "outbound-rule", "create")]
public record AzNetworkLbOutboundRuleCreateOptions(
[property: CliOption("--address-pool")] string AddressPool,
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--name")] string Name,
[property: CliOption("--protocol")] string Protocol,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--allocated-outbound-ports")]
    public string? AllocatedOutboundPorts { get; set; }

    [CliFlag("--enable-tcp-reset")]
    public bool? EnableTcpReset { get; set; }

    [CliOption("--frontend-ip-configs")]
    public string? FrontendIpConfigs { get; set; }

    [CliOption("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}