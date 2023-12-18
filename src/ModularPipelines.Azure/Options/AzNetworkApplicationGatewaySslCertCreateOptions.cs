using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "ssl-cert", "create")]
public record AzNetworkApplicationGatewaySslCertCreateOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--cert-file")]
    public string? CertFile { get; set; }

    [CommandSwitch("--cert-password")]
    public string? CertPassword { get; set; }

    [CommandSwitch("--key-vault-secret-id")]
    public string? KeyVaultSecretId { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}