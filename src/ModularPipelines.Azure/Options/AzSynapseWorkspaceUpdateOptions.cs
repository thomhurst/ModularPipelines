using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "workspace", "update")]
public record AzSynapseWorkspaceUpdateOptions : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliFlag("--allowed-tenant-ids")]
    public bool? AllowedTenantIds { get; set; }

    [CliOption("--collaboration-branch")]
    public string? CollaborationBranch { get; set; }

    [CliOption("--host-name")]
    public string? HostName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--key-name")]
    public string? KeyName { get; set; }

    [CliOption("--last-commit-id")]
    public string? LastCommitId { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--project-name")]
    public string? ProjectName { get; set; }

    [CliOption("--repository-name")]
    public string? RepositoryName { get; set; }

    [CliOption("--repository-type")]
    public string? RepositoryType { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--root-folder")]
    public string? RootFolder { get; set; }

    [CliOption("--sql-admin-login-password")]
    public string? SqlAdminLoginPassword { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }

    [CliOption("--uami-action")]
    public string? UamiAction { get; set; }

    [CliOption("--uami-id")]
    public string? UamiId { get; set; }

    [CliOption("--uami-id-in-encrypt")]
    public string? UamiIdInEncrypt { get; set; }

    [CliFlag("--use-sami-in-encrypt")]
    public bool? UseSamiInEncrypt { get; set; }
}