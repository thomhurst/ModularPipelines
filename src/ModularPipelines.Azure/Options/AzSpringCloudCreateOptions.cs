using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "create")]
public record AzSpringCloudCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
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

    [BooleanCommandSwitch("--enable-acs")]
    public bool? EnableAcs { get; set; }

    [BooleanCommandSwitch("--enable-api-portal")]
    public bool? EnableApiPortal { get; set; }

    [BooleanCommandSwitch("--enable-gateway")]
    public bool? EnableGateway { get; set; }

    [BooleanCommandSwitch("--enable-service-registry")]
    public bool? EnableServiceRegistry { get; set; }

    [CommandSwitch("--gateway-instance-count")]
    public int? GatewayInstanceCount { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

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