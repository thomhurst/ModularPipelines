using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "frontend-endpoint", "enable-https")]
public record AzNetworkFrontDoorFrontendEndpointEnableHttpsOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--certificate-source")]
    public string? CertificateSource { get; set; }

    [CliOption("--minimum-tls-version")]
    public string? MinimumTlsVersion { get; set; }

    [CliOption("--secret-name")]
    public string? SecretName { get; set; }

    [CliOption("--secret-version")]
    public string? SecretVersion { get; set; }

    [CliOption("--vault-id")]
    public string? VaultId { get; set; }
}