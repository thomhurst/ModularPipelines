using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vnet-gateway", "vpn-client", "generate")]
public record AzNetworkVnetGatewayVpnClientGenerateOptions : AzOptions
{
    [CommandSwitch("--authentication-method")]
    public string? AuthenticationMethod { get; set; }

    [CommandSwitch("--client-root-certificates")]
    public string? ClientRootCertificates { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--processor-architecture")]
    public string? ProcessorArchitecture { get; set; }

    [CommandSwitch("--radius-server-auth-certificate")]
    public string? RadiusServerAuthCertificate { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--use-legacy")]
    public bool? UseLegacy { get; set; }
}