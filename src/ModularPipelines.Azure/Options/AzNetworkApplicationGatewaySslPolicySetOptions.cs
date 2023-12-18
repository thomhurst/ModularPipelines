using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "ssl-policy", "set")]
public record AzNetworkApplicationGatewaySslPolicySetOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--cipher-suites")]
    public string? CipherSuites { get; set; }

    [BooleanCommandSwitch("--disabled-ssl-protocols")]
    public bool? DisabledSslProtocols { get; set; }

    [CommandSwitch("--min-protocol-version")]
    public string? MinProtocolVersion { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--policy-type")]
    public string? PolicyType { get; set; }
}

