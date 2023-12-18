using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "bastion", "rdp")]
public record AzNetworkBastionRdpOptions : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [BooleanCommandSwitch("--configure")]
    public bool? Configure { get; set; }

    [BooleanCommandSwitch("--disable-gateway")]
    public bool? DisableGateway { get; set; }

    [BooleanCommandSwitch("--enable-mfa")]
    public bool? EnableMfa { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-port")]
    public string? ResourcePort { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--target-ip-address")]
    public string? TargetIpAddress { get; set; }

    [CommandSwitch("--target-resource-id")]
    public string? TargetResourceId { get; set; }
}