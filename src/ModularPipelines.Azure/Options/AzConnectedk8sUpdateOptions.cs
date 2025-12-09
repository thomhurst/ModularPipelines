using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("connectedk8s", "update")]
public record AzConnectedk8sUpdateOptions : AzOptions
{
    [CliFlag("--auto-upgrade")]
    public bool? AutoUpgrade { get; set; }

    [CliFlag("--azure-hybrid-benefit")]
    public bool? AzureHybridBenefit { get; set; }

    [CliOption("--container-log-path")]
    public string? ContainerLogPath { get; set; }

    [CliOption("--custom-ca-cert")]
    public string? CustomCaCert { get; set; }

    [CliFlag("--disable-proxy")]
    public bool? DisableProxy { get; set; }

    [CliOption("--distribution")]
    public string? Distribution { get; set; }

    [CliOption("--distribution-version")]
    public string? DistributionVersion { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kube-config")]
    public string? KubeConfig { get; set; }

    [CliOption("--kube-context")]
    public string? KubeContext { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--proxy-http")]
    public string? ProxyHttp { get; set; }

    [CliOption("--proxy-https")]
    public string? ProxyHttps { get; set; }

    [CliOption("--proxy-skip-range")]
    public string? ProxySkipRange { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}