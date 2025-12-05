using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cdn", "custom-domain", "enable-https")]
public record AzCdnCustomDomainEnableHttpsOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--name")] string Name,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--min-tls-version")]
    public string? MinTlsVersion { get; set; }

    [CliOption("--user-cert-group-name")]
    public string? UserCertGroupName { get; set; }

    [CliOption("--user-cert-protocol-type")]
    public string? UserCertProtocolType { get; set; }

    [CliOption("--user-cert-secret-name")]
    public string? UserCertSecretName { get; set; }

    [CliOption("--user-cert-secret-version")]
    public string? UserCertSecretVersion { get; set; }

    [CliOption("--user-cert-subscription-id")]
    public string? UserCertSubscriptionId { get; set; }

    [CliOption("--user-cert-vault-name")]
    public string? UserCertVaultName { get; set; }
}