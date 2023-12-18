using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedk8s", "connect")]
public record AzConnectedk8sConnectOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--azure-hybrid-benefit")]
    public bool? AzureHybridBenefit { get; set; }

    [CommandSwitch("--container-log-path")]
    public string? ContainerLogPath { get; set; }

    [CommandSwitch("--correlation-id")]
    public string? CorrelationId { get; set; }

    [CommandSwitch("--custom-ca-cert")]
    public string? CustomCaCert { get; set; }

    [CommandSwitch("--custom-locations-oid")]
    public string? CustomLocationsOid { get; set; }

    [BooleanCommandSwitch("--disable-auto-upgrade")]
    public bool? DisableAutoUpgrade { get; set; }

    [CommandSwitch("--distribution")]
    public string? Distribution { get; set; }

    [CommandSwitch("--distribution-version")]
    public string? DistributionVersion { get; set; }

    [BooleanCommandSwitch("--enable-private-link")]
    public bool? EnablePrivateLink { get; set; }

    [CommandSwitch("--infrastructure")]
    public string? Infrastructure { get; set; }

    [CommandSwitch("--kube-config")]
    public string? KubeConfig { get; set; }

    [CommandSwitch("--kube-context")]
    public string? KubeContext { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--onboarding-timeout")]
    public string? OnboardingTimeout { get; set; }

    [CommandSwitch("--pls-arm-id")]
    public string? PlsArmId { get; set; }

    [CommandSwitch("--proxy-http")]
    public string? ProxyHttp { get; set; }

    [CommandSwitch("--proxy-https")]
    public string? ProxyHttps { get; set; }

    [CommandSwitch("--proxy-skip-range")]
    public string? ProxySkipRange { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}