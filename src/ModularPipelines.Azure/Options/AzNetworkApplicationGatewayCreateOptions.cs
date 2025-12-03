using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "create")]
public record AzNetworkApplicationGatewayCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--capacity")]
    public string? Capacity { get; set; }

    [CliOption("--cert-file")]
    public string? CertFile { get; set; }

    [CliOption("--cert-password")]
    public string? CertPassword { get; set; }

    [CliOption("--connection-draining-timeout")]
    public string? ConnectionDrainingTimeout { get; set; }

    [CliOption("--custom-error-pages")]
    public string? CustomErrorPages { get; set; }

    [CliFlag("--enable-private-link")]
    public bool? EnablePrivateLink { get; set; }

    [CliOption("--frontend-port")]
    public string? FrontendPort { get; set; }

    [CliOption("--http-settings-cookie-based-affinity")]
    public string? HttpSettingsCookieBasedAffinity { get; set; }

    [CliOption("--http-settings-port")]
    public string? HttpSettingsPort { get; set; }

    [CliOption("--http-settings-protocol")]
    public string? HttpSettingsProtocol { get; set; }

    [CliOption("--http2")]
    public string? Http2 { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--key-vault-secret-id")]
    public string? KeyVaultSecretId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--max-capacity")]
    public string? MaxCapacity { get; set; }

    [CliOption("--min-capacity")]
    public string? MinCapacity { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CliOption("--private-link-ip-address")]
    public string? PrivateLinkIpAddress { get; set; }

    [CliFlag("--private-link-primary")]
    public bool? PrivateLinkPrimary { get; set; }

    [CliOption("--private-link-subnet")]
    public string? PrivateLinkSubnet { get; set; }

    [CliOption("--private-link-subnet-prefix")]
    public string? PrivateLinkSubnetPrefix { get; set; }

    [CliOption("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CliOption("--public-ip-address-allocation")]
    public string? PublicIpAddressAllocation { get; set; }

    [CliOption("--routing-rule-type")]
    public string? RoutingRuleType { get; set; }

    [CliOption("--servers")]
    public string? Servers { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--ssl-certificate-name")]
    public string? SslCertificateName { get; set; }

    [CliOption("--ssl-profile")]
    public string? SslProfile { get; set; }

    [CliOption("--ssl-profile-id")]
    public string? SslProfileId { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--subnet-address-prefix")]
    public string? SubnetAddressPrefix { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--trusted-client-cert")]
    public string? TrustedClientCert { get; set; }

    [CliFlag("--validate")]
    public bool? Validate { get; set; }

    [CliOption("--vnet-address-prefix")]
    public string? VnetAddressPrefix { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }

    [CliOption("--waf-policy")]
    public string? WafPolicy { get; set; }

    [CliOption("--zones")]
    public string? Zones { get; set; }
}