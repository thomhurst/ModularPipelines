using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "create")]
public record AzSpringCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--acs-gen")]
    public string? AcsGen { get; set; }

    [CommandSwitch("--ap-instance")]
    public string? ApInstance { get; set; }

    [CommandSwitch("--app-insights")]
    public string? AppInsights { get; set; }

    [CommandSwitch("--app-insights-key")]
    public string? AppInsightsKey { get; set; }

    [CommandSwitch("--app-network-resource-group")]
    public string? AppNetworkResourceGroup { get; set; }

    [CommandSwitch("--app-subnet")]
    public string? AppSubnet { get; set; }

    [CommandSwitch("--build-pool-size")]
    public string? BuildPoolSize { get; set; }

    [BooleanCommandSwitch("--disable-app-insights")]
    public bool? DisableAppInsights { get; set; }

    [BooleanCommandSwitch("--disable-build-service")]
    public bool? DisableBuildService { get; set; }

    [BooleanCommandSwitch("--enable-acs")]
    public bool? EnableAcs { get; set; }

    [BooleanCommandSwitch("--enable-alv")]
    public bool? EnableAlv { get; set; }

    [BooleanCommandSwitch("--enable-api-portal")]
    public bool? EnableApiPortal { get; set; }

    [BooleanCommandSwitch("--enable-app-acc")]
    public bool? EnableAppAcc { get; set; }

    [BooleanCommandSwitch("--enable-dataplane-public-endpoint")]
    public bool? EnableDataplanePublicEndpoint { get; set; }

    [BooleanCommandSwitch("--enable-gateway")]
    public bool? EnableGateway { get; set; }

    [BooleanCommandSwitch("--enable-service-registry")]
    public bool? EnableServiceRegistry { get; set; }

    [CommandSwitch("--gateway-instance-count")]
    public int? GatewayInstanceCount { get; set; }

    [CommandSwitch("--infra-resource-group")]
    public string? InfraResourceGroup { get; set; }

    [CommandSwitch("--ingress-read-timeout")]
    public string? IngressReadTimeout { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-environment")]
    public string? ManagedEnvironment { get; set; }

    [CommandSwitch("--marketplace-plan-id")]
    public string? MarketplacePlanId { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--outbound-type")]
    public string? OutboundType { get; set; }

    [CommandSwitch("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CommandSwitch("--registry-server")]
    public string? RegistryServer { get; set; }

    [CommandSwitch("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CommandSwitch("--reserved-cidr-range")]
    public string? ReservedCidrRange { get; set; }

    [CommandSwitch("--sampling-rate")]
    public string? SamplingRate { get; set; }

    [CommandSwitch("--service-runtime-network-resource-group")]
    public string? ServiceRuntimeNetworkResourceGroup { get; set; }

    [CommandSwitch("--service-runtime-subnet")]
    public string? ServiceRuntimeSubnet { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vnet")]
    public string? Vnet { get; set; }

    [BooleanCommandSwitch("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}