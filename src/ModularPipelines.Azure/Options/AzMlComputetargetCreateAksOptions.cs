using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "computetarget", "create", "aks")]
public record AzMlComputetargetCreateAksOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--agent-count")]
    public int? AgentCount { get; set; }

    [CliOption("--cluster-purpose")]
    public string? ClusterPurpose { get; set; }

    [CliOption("--dns-service-ip")]
    public string? DnsServiceIp { get; set; }

    [CliOption("--docker-bridge-cidr")]
    public string? DockerBridgeCidr { get; set; }

    [CliOption("--load-balancer-subnet")]
    public string? LoadBalancerSubnet { get; set; }

    [CliOption("--load-balancer-type")]
    public string? LoadBalancerType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service-cidr")]
    public string? ServiceCidr { get; set; }

    [CliOption("--ssl-cert-file")]
    public string? SslCertFile { get; set; }

    [CliOption("--ssl-cname")]
    public string? SslCname { get; set; }

    [CliOption("--ssl-key-file")]
    public string? SslKeyFile { get; set; }

    [CliOption("--ssl-leaf-domain-label")]
    public string? SslLeafDomainLabel { get; set; }

    [CliOption("--ssl-overwrite-domain")]
    public string? SslOverwriteDomain { get; set; }

    [CliOption("--subnet-name")]
    public string? SubnetName { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--vm-size")]
    public string? VmSize { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }

    [CliOption("--vnet-resourcegroup-name")]
    public string? VnetResourcegroupName { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliFlag("-v")]
    public bool? V { get; set; }
}