using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb", "outbound-rule", "update")]
public record AzNetworkLbOutboundRuleUpdateOptions(
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--address-pool")]
    public string? AddressPool { get; set; }

    [CommandSwitch("--allocated-outbound-ports")]
    public string? AllocatedOutboundPorts { get; set; }

    [BooleanCommandSwitch("--enable-tcp-reset")]
    public bool? EnableTcpReset { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--frontend-ip-configs")]
    public string? FrontendIpConfigs { get; set; }

    [CommandSwitch("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }
}

