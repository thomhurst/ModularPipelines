using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "custom-domain", "delete")]
public record AzCdnCustomDomainDeleteOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--min-tls-version")]
    public string? MinTlsVersion { get; set; }

    [CommandSwitch("--user-cert-group-name")]
    public string? UserCertGroupName { get; set; }

    [CommandSwitch("--user-cert-protocol-type")]
    public string? UserCertProtocolType { get; set; }

    [CommandSwitch("--user-cert-secret-name")]
    public string? UserCertSecretName { get; set; }

    [CommandSwitch("--user-cert-secret-version")]
    public string? UserCertSecretVersion { get; set; }

    [CommandSwitch("--user-cert-subscription-id")]
    public string? UserCertSubscriptionId { get; set; }

    [CommandSwitch("--user-cert-vault-name")]
    public string? UserCertVaultName { get; set; }
}

