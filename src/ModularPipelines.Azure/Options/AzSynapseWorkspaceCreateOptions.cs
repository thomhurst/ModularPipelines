using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "workspace", "create")]
public record AzSynapseWorkspaceCreateOptions(
[property: CliOption("--file-system")] string FileSystem,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sql-admin-login-password")] string SqlAdminLoginPassword,
[property: CliOption("--sql-admin-login-user")] string SqlAdminLoginUser,
[property: CliOption("--storage-account")] int StorageAccount
) : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliFlag("--allowed-tenant-ids")]
    public bool? AllowedTenantIds { get; set; }

    [CliOption("--cmk")]
    public string? Cmk { get; set; }

    [CliOption("--collaboration-branch")]
    public string? CollaborationBranch { get; set; }

    [CliFlag("--enable-managed-virtual-network")]
    public bool? EnableManagedVirtualNetwork { get; set; }

    [CliOption("--host-name")]
    public string? HostName { get; set; }

    [CliOption("--key-name")]
    public string? KeyName { get; set; }

    [CliOption("--last-commit-id")]
    public string? LastCommitId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-rg-name")]
    public string? ManagedRgName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--prevent-data-exfiltration")]
    public bool? PreventDataExfiltration { get; set; }

    [CliOption("--project-name")]
    public string? ProjectName { get; set; }

    [CliOption("--repository-name")]
    public string? RepositoryName { get; set; }

    [CliOption("--repository-type")]
    public string? RepositoryType { get; set; }

    [CliOption("--root-folder")]
    public string? RootFolder { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }

    [CliOption("--uami-id")]
    public string? UamiId { get; set; }

    [CliOption("--uami-id-in-encrypt")]
    public string? UamiIdInEncrypt { get; set; }

    [CliFlag("--use-sami-in-encrypt")]
    public bool? UseSamiInEncrypt { get; set; }
}