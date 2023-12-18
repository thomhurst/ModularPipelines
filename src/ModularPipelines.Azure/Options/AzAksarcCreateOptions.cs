using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aksarc", "create")]
public record AzAksarcCreateOptions(
[property: CommandSwitch("--custom-location")] string CustomLocation,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aad-admin-group-object-ids")]
    public string? AadAdminGroupObjectIds { get; set; }

    [CommandSwitch("--control-plane-ip")]
    public string? ControlPlaneIp { get; set; }

    [CommandSwitch("--control-plane-node-count")]
    public int? ControlPlaneNodeCount { get; set; }

    [CommandSwitch("--control-plane-vm-size")]
    public string? ControlPlaneVmSize { get; set; }

    [BooleanCommandSwitch("--enable-ahub")]
    public bool? EnableAhub { get; set; }

    [BooleanCommandSwitch("--generate-ssh-keys")]
    public bool? GenerateSshKeys { get; set; }

    [CommandSwitch("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CommandSwitch("--load-balancer-count")]
    public int? LoadBalancerCount { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--max-pods")]
    public string? MaxPods { get; set; }

    [BooleanCommandSwitch("--no-ssh-key")]
    public bool? NoSshKey { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--node-count")]
    public int? NodeCount { get; set; }

    [CommandSwitch("--node-vm-size")]
    public string? NodeVmSize { get; set; }

    [CommandSwitch("--nodepool-labels")]
    public string? NodepoolLabels { get; set; }

    [CommandSwitch("--nodepool-taints")]
    public string? NodepoolTaints { get; set; }

    [CommandSwitch("--pod-cidr")]
    public string? PodCidr { get; set; }

    [CommandSwitch("--ssh-authorized-ip-ranges")]
    public string? SshAuthorizedIpRanges { get; set; }

    [CommandSwitch("--ssh-key-value")]
    public string? SshKeyValue { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vnet-ids")]
    public string? VnetIds { get; set; }
}