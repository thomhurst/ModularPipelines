using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "computetarget", "create", "amlcompute")]
public record AzMlComputetargetCreateAmlcomputeOptions(
[property: CliOption("--max-nodes")] string MaxNodes,
[property: CliOption("--name")] string Name,
[property: CliOption("--vm-size")] string VmSize
) : AzOptions
{
    [CliOption("--admin-user-password")]
    public string? AdminUserPassword { get; set; }

    [CliOption("--admin-user-ssh-key")]
    public string? AdminUserSshKey { get; set; }

    [CliOption("--admin-username")]
    public string? AdminUsername { get; set; }

    [CliOption("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-node-public-ip")]
    public bool? EnableNodePublicIp { get; set; }

    [CliOption("--idle-seconds-before-scaledown")]
    public string? IdleSecondsBeforeScaledown { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--min-nodes")]
    public string? MinNodes { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--remote-login-port-public-access")]
    public string? RemoteLoginPortPublicAccess { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subnet-name")]
    public string? SubnetName { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--tag")]
    public string? Tag { get; set; }

    [CliOption("--vm-priority")]
    public string? VmPriority { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }

    [CliOption("--vnet-resourcegroup-name")]
    public string? VnetResourcegroupName { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliFlag("-v")]
    public bool? V { get; set; }
}