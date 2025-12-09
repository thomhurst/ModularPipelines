using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "computetarget", "create", "computeinstance")]
public record AzMlComputetargetCreateComputeinstanceOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--vm-size")] string VmSize
) : AzOptions
{
    [CliOption("--admin-user-ssh-public-key")]
    public string? AdminUserSshPublicKey { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--ssh-public-access")]
    public string? SshPublicAccess { get; set; }

    [CliOption("--subnet-name")]
    public string? SubnetName { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--tag")]
    public string? Tag { get; set; }

    [CliOption("--user-object-id")]
    public string? UserObjectId { get; set; }

    [CliOption("--user-tenant-id")]
    public string? UserTenantId { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }

    [CliOption("--vnet-resourcegroup-name")]
    public string? VnetResourcegroupName { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliFlag("-v")]
    public bool? V { get; set; }
}