using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "workspace", "check-name")]
public record AzSynapseWorkspaceCheckNameOptions(
[property: CommandSwitch("--name")] string Name
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

    [CommandSwitch("--use-sami-in-encrypt")]
    public string? UseSamiInEncrypt { get; set; }
}

