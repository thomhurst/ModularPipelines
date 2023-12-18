using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "create")]
public record AzNetworkApplicationGatewayCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--capacity")]
    public string? Capacity { get; set; }

    [CommandSwitch("--cert-file")]
    public string? CertFile { get; set; }

    [CommandSwitch("--cert-password")]
    public string? CertPassword { get; set; }

    [CommandSwitch("--connection-draining-timeout")]
    public string? ConnectionDrainingTimeout { get; set; }

    [CommandSwitch("--custom-error-pages")]
    public string? CustomErrorPages { get; set; }

    [BooleanCommandSwitch("--enable-private-link")]
    public bool? EnablePrivateLink { get; set; }

    [CommandSwitch("--frontend-port")]
    public string? FrontendPort { get; set; }

    [CommandSwitch("--http-settings-cookie-based-affinity")]
    public string? HttpSettingsCookieBasedAffinity { get; set; }

    [CommandSwitch("--http-settings-port")]
    public string? HttpSettingsPort { get; set; }

    [CommandSwitch("--http-settings-protocol")]
    public string? HttpSettingsProtocol { get; set; }

    [CommandSwitch("--http2")]
    public string? Http2 { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--key-vault-secret-id")]
    public string? KeyVaultSecretId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--max-capacity")]
    public string? MaxCapacity { get; set; }

    [CommandSwitch("--min-capacity")]
    public string? MinCapacity { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CommandSwitch("--private-link-ip-address")]
    public string? PrivateLinkIpAddress { get; set; }

    [BooleanCommandSwitch("--private-link-primary")]
    public bool? PrivateLinkPrimary { get; set; }

    [CommandSwitch("--private-link-subnet")]
    public string? PrivateLinkSubnet { get; set; }

    [CommandSwitch("--private-link-subnet-prefix")]
    public string? PrivateLinkSubnetPrefix { get; set; }

    [CommandSwitch("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CommandSwitch("--public-ip-address-allocation")]
    public string? PublicIpAddressAllocation { get; set; }

    [CommandSwitch("--routing-rule-type")]
    public string? RoutingRuleType { get; set; }

    [CommandSwitch("--servers")]
    public string? Servers { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--ssl-certificate-name")]
    public string? SslCertificateName { get; set; }

    [CommandSwitch("--ssl-profile")]
    public string? SslProfile { get; set; }

    [CommandSwitch("--ssl-profile-id")]
    public string? SslProfileId { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--subnet-address-prefix")]
    public string? SubnetAddressPrefix { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--trusted-client-cert")]
    public string? TrustedClientCert { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }

    [CommandSwitch("--vnet-address-prefix")]
    public string? VnetAddressPrefix { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }

    [CommandSwitch("--waf-policy")]
    public string? WafPolicy { get; set; }

    [CommandSwitch("--zones")]
    public string? Zones { get; set; }
}

