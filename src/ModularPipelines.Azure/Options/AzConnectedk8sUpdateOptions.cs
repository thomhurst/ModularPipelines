using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedk8s", "update")]
public record AzConnectedk8sUpdateOptions : AzOptions
{
    [BooleanCommandSwitch("--auto-upgrade")]
    public bool? AutoUpgrade { get; set; }

    [BooleanCommandSwitch("--azure-hybrid-benefit")]
    public bool? AzureHybridBenefit { get; set; }

    [CommandSwitch("--container-log-path")]
    public string? ContainerLogPath { get; set; }

    [CommandSwitch("--custom-ca-cert")]
    public string? CustomCaCert { get; set; }

    [BooleanCommandSwitch("--disable-proxy")]
    public bool? DisableProxy { get; set; }

    [CommandSwitch("--distribution")]
    public string? Distribution { get; set; }

    [CommandSwitch("--distribution-version")]
    public string? DistributionVersion { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--kube-config")]
    public string? KubeConfig { get; set; }

    [CommandSwitch("--kube-context")]
    public string? KubeContext { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--proxy-http")]
    public string? ProxyHttp { get; set; }

    [CommandSwitch("--proxy-https")]
    public string? ProxyHttps { get; set; }

    [CommandSwitch("--proxy-skip-range")]
    public string? ProxySkipRange { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}