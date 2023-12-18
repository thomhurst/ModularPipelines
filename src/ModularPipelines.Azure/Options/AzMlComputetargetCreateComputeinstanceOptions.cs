using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "computetarget", "create", "computeinstance")]
public record AzMlComputetargetCreateComputeinstanceOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--vm-size")] string VmSize
) : AzOptions
{
    [CommandSwitch("--admin-user-ssh-public-key")]
    public string? AdminUserSshPublicKey { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--ssh-public-access")]
    public string? SshPublicAccess { get; set; }

    [CommandSwitch("--subnet-name")]
    public string? SubnetName { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--user-object-id")]
    public string? UserObjectId { get; set; }

    [CommandSwitch("--user-tenant-id")]
    public string? UserTenantId { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }

    [CommandSwitch("--vnet-resourcegroup-name")]
    public string? VnetResourcegroupName { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [BooleanCommandSwitch("-v")]
    public bool? V { get; set; }
}