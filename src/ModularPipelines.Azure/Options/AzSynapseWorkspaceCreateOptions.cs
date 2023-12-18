using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "workspace", "create")]
public record AzSynapseWorkspaceCreateOptions(
[property: CommandSwitch("--file-system")] string FileSystem,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sql-admin-login-password")] string SqlAdminLoginPassword,
[property: CommandSwitch("--sql-admin-login-user")] string SqlAdminLoginUser,
[property: CommandSwitch("--storage-account")] int StorageAccount
) : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [BooleanCommandSwitch("--allowed-tenant-ids")]
    public bool? AllowedTenantIds { get; set; }

    [CommandSwitch("--cmk")]
    public string? Cmk { get; set; }

    [CommandSwitch("--collaboration-branch")]
    public string? CollaborationBranch { get; set; }

    [BooleanCommandSwitch("--enable-managed-virtual-network")]
    public bool? EnableManagedVirtualNetwork { get; set; }

    [CommandSwitch("--host-name")]
    public string? HostName { get; set; }

    [CommandSwitch("--key-name")]
    public string? KeyName { get; set; }

    [CommandSwitch("--last-commit-id")]
    public string? LastCommitId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-rg-name")]
    public string? ManagedRgName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--prevent-data-exfiltration")]
    public bool? PreventDataExfiltration { get; set; }

    [CommandSwitch("--project-name")]
    public string? ProjectName { get; set; }

    [CommandSwitch("--repository-name")]
    public string? RepositoryName { get; set; }

    [CommandSwitch("--repository-type")]
    public string? RepositoryType { get; set; }

    [CommandSwitch("--root-folder")]
    public string? RootFolder { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tenant-id")]
    public string? TenantId { get; set; }

    [CommandSwitch("--uami-id")]
    public string? UamiId { get; set; }

    [CommandSwitch("--uami-id-in-encrypt")]
    public string? UamiIdInEncrypt { get; set; }

    [BooleanCommandSwitch("--use-sami-in-encrypt")]
    public bool? UseSamiInEncrypt { get; set; }
}