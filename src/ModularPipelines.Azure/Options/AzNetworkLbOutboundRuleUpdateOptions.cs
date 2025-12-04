using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "lb", "outbound-rule", "update")]
public record AzNetworkLbOutboundRuleUpdateOptions(
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--address-pool")]
    public string? AddressPool { get; set; }

    [CliOption("--allocated-outbound-ports")]
    public string? AllocatedOutboundPorts { get; set; }

    [CliFlag("--enable-tcp-reset")]
    public bool? EnableTcpReset { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--frontend-ip-configs")]
    public string? FrontendIpConfigs { get; set; }

    [CliOption("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}