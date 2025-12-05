using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "ssl-cert", "create")]
public record AzNetworkApplicationGatewaySslCertCreateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--cert-file")]
    public string? CertFile { get; set; }

    [CliOption("--cert-password")]
    public string? CertPassword { get; set; }

    [CliOption("--key-vault-secret-id")]
    public string? KeyVaultSecretId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}