using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "computetarget", "create", "aks")]
public record AzMlComputetargetCreateAksOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--agent-count")]
    public int? AgentCount { get; set; }

    [CommandSwitch("--cluster-purpose")]
    public string? ClusterPurpose { get; set; }

    [CommandSwitch("--dns-service-ip")]
    public string? DnsServiceIp { get; set; }

    [CommandSwitch("--docker-bridge-cidr")]
    public string? DockerBridgeCidr { get; set; }

    [CommandSwitch("--load-balancer-subnet")]
    public string? LoadBalancerSubnet { get; set; }

    [CommandSwitch("--load-balancer-type")]
    public string? LoadBalancerType { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--service-cidr")]
    public string? ServiceCidr { get; set; }

    [CommandSwitch("--ssl-cert-file")]
    public string? SslCertFile { get; set; }

    [CommandSwitch("--ssl-cname")]
    public string? SslCname { get; set; }

    [CommandSwitch("--ssl-key-file")]
    public string? SslKeyFile { get; set; }

    [CommandSwitch("--ssl-leaf-domain-label")]
    public string? SslLeafDomainLabel { get; set; }

    [CommandSwitch("--ssl-overwrite-domain")]
    public string? SslOverwriteDomain { get; set; }

    [CommandSwitch("--subnet-name")]
    public string? SubnetName { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--vm-size")]
    public string? VmSize { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }

    [CommandSwitch("--vnet-resourcegroup-name")]
    public string? VnetResourcegroupName { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [BooleanCommandSwitch("-v")]
    public bool? V { get; set; }
}

