using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "computetarget", "update", "aks")]
public record AzMlComputetargetUpdateAksOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--load-balancer-subnet")]
    public string? LoadBalancerSubnet { get; set; }

    [CommandSwitch("--load-balancer-type")]
    public string? LoadBalancerType { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--ssl-cert-file")]
    public string? SslCertFile { get; set; }

    [CommandSwitch("--ssl-cname")]
    public string? SslCname { get; set; }

    [CommandSwitch("--ssl-disable")]
    public string? SslDisable { get; set; }

    [CommandSwitch("--ssl-key-file")]
    public string? SslKeyFile { get; set; }

    [CommandSwitch("--ssl-leaf-domain-label")]
    public string? SslLeafDomainLabel { get; set; }

    [CommandSwitch("--ssl-overwrite-domain")]
    public string? SslOverwriteDomain { get; set; }

    [CommandSwitch("--ssl-renew")]
    public string? SslRenew { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [BooleanCommandSwitch("-v")]
    public bool? V { get; set; }
}