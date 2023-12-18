using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hybridaks", "create")]
public record AzHybridaksCreateOptions(
[property: CommandSwitch("--custom-location")] string CustomLocation,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aad-admin-group-object-ids")]
    public string? AadAdminGroupObjectIds { get; set; }

    [CommandSwitch("--addon")]
    public string? Addon { get; set; }

    [CommandSwitch("--arc-agent-auto-upgrade")]
    public string? ArcAgentAutoUpgrade { get; set; }

    [CommandSwitch("--arc-agent-version")]
    public string? ArcAgentVersion { get; set; }

    [CommandSwitch("--control-plane-count")]
    public int? ControlPlaneCount { get; set; }

    [CommandSwitch("--control-plane-ip")]
    public string? ControlPlaneIp { get; set; }

    [CommandSwitch("--control-plane-vm-size")]
    public string? ControlPlaneVmSize { get; set; }

    [BooleanCommandSwitch("--generate-ssh-keys")]
    public bool? GenerateSshKeys { get; set; }

    [CommandSwitch("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CommandSwitch("--load-balancer-count")]
    public int? LoadBalancerCount { get; set; }

    [CommandSwitch("--load-balancer-sku")]
    public string? LoadBalancerSku { get; set; }

    [CommandSwitch("--load-balancer-vm-size")]
    public string? LoadBalancerVmSize { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-ssh-key")]
    public bool? NoSshKey { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--node-count")]
    public int? NodeCount { get; set; }

    [CommandSwitch("--node-vm-size")]
    public string? NodeVmSize { get; set; }

    [CommandSwitch("--pod-cidr")]
    public string? PodCidr { get; set; }

    [CommandSwitch("--service-cidr")]
    public string? ServiceCidr { get; set; }

    [CommandSwitch("--ssh-key-value")]
    public string? SshKeyValue { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vnet-ids")]
    public string? VnetIds { get; set; }

    [CommandSwitch("--zones")]
    public string? Zones { get; set; }
}

