using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "compute", "create")]
public record AzMlComputeCreateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliOption("--admin-username")]
    public string? AdminUsername { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-node-public-ip")]
    public bool? EnableNodePublicIp { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--idle-time-before-scale-down")]
    public string? IdleTimeBeforeScaleDown { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--max-instances")]
    public string? MaxInstances { get; set; }

    [CliOption("--min-instances")]
    public string? MinInstances { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--size")]
    public string? Size { get; set; }

    [CliOption("--ssh-key-value")]
    public string? SshKeyValue { get; set; }

    [CliFlag("--ssh-public-access-enabled")]
    public bool? SshPublicAccessEnabled { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--user-assigned-identities")]
    public string? UserAssignedIdentities { get; set; }

    [CliOption("--user-object-id")]
    public string? UserObjectId { get; set; }

    [CliOption("--user-tenant-id")]
    public string? UserTenantId { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}