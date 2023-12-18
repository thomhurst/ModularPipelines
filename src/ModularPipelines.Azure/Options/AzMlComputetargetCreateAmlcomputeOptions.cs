using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "computetarget", "create", "amlcompute")]
public record AzMlComputetargetCreateAmlcomputeOptions(
[property: CommandSwitch("--max-nodes")] string MaxNodes,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--vm-size")] string VmSize
) : AzOptions
{
    [CommandSwitch("--admin-user-password")]
    public string? AdminUserPassword { get; set; }

    [CommandSwitch("--admin-user-ssh-key")]
    public string? AdminUserSshKey { get; set; }

    [CommandSwitch("--admin-username")]
    public string? AdminUsername { get; set; }

    [CommandSwitch("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-node-public-ip")]
    public bool? EnableNodePublicIp { get; set; }

    [CommandSwitch("--idle-seconds-before-scaledown")]
    public string? IdleSecondsBeforeScaledown { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--min-nodes")]
    public string? MinNodes { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--remote-login-port-public-access")]
    public string? RemoteLoginPortPublicAccess { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subnet-name")]
    public string? SubnetName { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--vm-priority")]
    public string? VmPriority { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }

    [CommandSwitch("--vnet-resourcegroup-name")]
    public string? VnetResourcegroupName { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [BooleanCommandSwitch("-v")]
    public bool? V { get; set; }
}