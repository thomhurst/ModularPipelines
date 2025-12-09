using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "root-cert", "create")]
public record AzNetworkApplicationGatewayRootCertCreateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--cert-file")]
    public string? CertFile { get; set; }

    [CliOption("--keyvault-secret")]
    public string? KeyvaultSecret { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}