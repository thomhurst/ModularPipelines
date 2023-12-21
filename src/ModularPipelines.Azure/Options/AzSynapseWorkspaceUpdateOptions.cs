using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "workspace", "update")]
public record AzSynapseWorkspaceUpdateOptions : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [BooleanCommandSwitch("--allowed-tenant-ids")]
    public bool? AllowedTenantIds { get; set; }

    [CommandSwitch("--collaboration-branch")]
    public string? CollaborationBranch { get; set; }

    [CommandSwitch("--host-name")]
    public string? HostName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--key-name")]
    public string? KeyName { get; set; }

    [CommandSwitch("--last-commit-id")]
    public string? LastCommitId { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--project-name")]
    public string? ProjectName { get; set; }

    [CommandSwitch("--repository-name")]
    public string? RepositoryName { get; set; }

    [CommandSwitch("--repository-type")]
    public string? RepositoryType { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--root-folder")]
    public string? RootFolder { get; set; }

    [CommandSwitch("--sql-admin-login-password")]
    public string? SqlAdminLoginPassword { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tenant-id")]
    public string? TenantId { get; set; }

    [CommandSwitch("--uami-action")]
    public string? UamiAction { get; set; }

    [CommandSwitch("--uami-id")]
    public string? UamiId { get; set; }

    [CommandSwitch("--uami-id-in-encrypt")]
    public string? UamiIdInEncrypt { get; set; }

    [BooleanCommandSwitch("--use-sami-in-encrypt")]
    public bool? UseSamiInEncrypt { get; set; }
}