using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vnet-gateway", "vpn-client", "generate")]
public record AzNetworkVnetGatewayVpnClientGenerateOptions : AzOptions
{
    [CliOption("--authentication-method")]
    public string? AuthenticationMethod { get; set; }

    [CliOption("--client-root-certificates")]
    public string? ClientRootCertificates { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--processor-architecture")]
    public string? ProcessorArchitecture { get; set; }

    [CliOption("--radius-server-auth-certificate")]
    public string? RadiusServerAuthCertificate { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--use-legacy")]
    public bool? UseLegacy { get; set; }
}