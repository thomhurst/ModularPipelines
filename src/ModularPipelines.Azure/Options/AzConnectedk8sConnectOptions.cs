using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("connectedk8s", "connect")]
public record AzConnectedk8sConnectOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--azure-hybrid-benefit")]
    public bool? AzureHybridBenefit { get; set; }

    [CliOption("--container-log-path")]
    public string? ContainerLogPath { get; set; }

    [CliOption("--correlation-id")]
    public string? CorrelationId { get; set; }

    [CliOption("--custom-ca-cert")]
    public string? CustomCaCert { get; set; }

    [CliOption("--custom-locations-oid")]
    public string? CustomLocationsOid { get; set; }

    [CliFlag("--disable-auto-upgrade")]
    public bool? DisableAutoUpgrade { get; set; }

    [CliOption("--distribution")]
    public string? Distribution { get; set; }

    [CliOption("--distribution-version")]
    public string? DistributionVersion { get; set; }

    [CliFlag("--enable-private-link")]
    public bool? EnablePrivateLink { get; set; }

    [CliOption("--infrastructure")]
    public string? Infrastructure { get; set; }

    [CliOption("--kube-config")]
    public string? KubeConfig { get; set; }

    [CliOption("--kube-context")]
    public string? KubeContext { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--onboarding-timeout")]
    public string? OnboardingTimeout { get; set; }

    [CliOption("--pls-arm-id")]
    public string? PlsArmId { get; set; }

    [CliOption("--proxy-http")]
    public string? ProxyHttp { get; set; }

    [CliOption("--proxy-https")]
    public string? ProxyHttps { get; set; }

    [CliOption("--proxy-skip-range")]
    public string? ProxySkipRange { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}