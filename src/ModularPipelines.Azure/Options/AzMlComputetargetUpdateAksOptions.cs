using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "computetarget", "update", "aks")]
public record AzMlComputetargetUpdateAksOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--load-balancer-subnet")]
    public string? LoadBalancerSubnet { get; set; }

    [CliOption("--load-balancer-type")]
    public string? LoadBalancerType { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--ssl-cert-file")]
    public string? SslCertFile { get; set; }

    [CliOption("--ssl-cname")]
    public string? SslCname { get; set; }

    [CliFlag("--ssl-disable")]
    public bool? SslDisable { get; set; }

    [CliOption("--ssl-key-file")]
    public string? SslKeyFile { get; set; }

    [CliOption("--ssl-leaf-domain-label")]
    public string? SslLeafDomainLabel { get; set; }

    [CliOption("--ssl-overwrite-domain")]
    public string? SslOverwriteDomain { get; set; }

    [CliOption("--ssl-renew")]
    public string? SslRenew { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliFlag("-v")]
    public bool? V { get; set; }
}