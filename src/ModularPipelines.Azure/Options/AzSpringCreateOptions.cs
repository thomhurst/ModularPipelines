using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "create")]
public record AzSpringCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--acs-gen")]
    public string? AcsGen { get; set; }

    [CliOption("--ap-instance")]
    public string? ApInstance { get; set; }

    [CliOption("--app-insights")]
    public string? AppInsights { get; set; }

    [CliOption("--app-insights-key")]
    public string? AppInsightsKey { get; set; }

    [CliOption("--app-network-resource-group")]
    public string? AppNetworkResourceGroup { get; set; }

    [CliOption("--app-subnet")]
    public string? AppSubnet { get; set; }

    [CliOption("--build-pool-size")]
    public string? BuildPoolSize { get; set; }

    [CliFlag("--disable-app-insights")]
    public bool? DisableAppInsights { get; set; }

    [CliFlag("--disable-build-service")]
    public bool? DisableBuildService { get; set; }

    [CliFlag("--enable-acs")]
    public bool? EnableAcs { get; set; }

    [CliFlag("--enable-alv")]
    public bool? EnableAlv { get; set; }

    [CliFlag("--enable-api-portal")]
    public bool? EnableApiPortal { get; set; }

    [CliFlag("--enable-app-acc")]
    public bool? EnableAppAcc { get; set; }

    [CliFlag("--enable-dataplane-public-endpoint")]
    public bool? EnableDataplanePublicEndpoint { get; set; }

    [CliFlag("--enable-gateway")]
    public bool? EnableGateway { get; set; }

    [CliFlag("--enable-service-registry")]
    public bool? EnableServiceRegistry { get; set; }

    [CliOption("--gateway-instance-count")]
    public int? GatewayInstanceCount { get; set; }

    [CliOption("--infra-resource-group")]
    public string? InfraResourceGroup { get; set; }

    [CliOption("--ingress-read-timeout")]
    public string? IngressReadTimeout { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-environment")]
    public string? ManagedEnvironment { get; set; }

    [CliOption("--marketplace-plan-id")]
    public string? MarketplacePlanId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--outbound-type")]
    public string? OutboundType { get; set; }

    [CliOption("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CliOption("--registry-server")]
    public string? RegistryServer { get; set; }

    [CliOption("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CliOption("--reserved-cidr-range")]
    public string? ReservedCidrRange { get; set; }

    [CliOption("--sampling-rate")]
    public string? SamplingRate { get; set; }

    [CliOption("--service-runtime-network-resource-group")]
    public string? ServiceRuntimeNetworkResourceGroup { get; set; }

    [CliOption("--service-runtime-subnet")]
    public string? ServiceRuntimeSubnet { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }

    [CliFlag("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}