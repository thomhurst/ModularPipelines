using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "restore", "restore-urefileshare")]
public record AzBackupRestoreRestoreAzurefileshareOptions(
[property: CommandSwitch("--resolve-conflict")] string ResolveConflict,
[property: CommandSwitch("--restore-mode")] string RestoreMode
) : AzOptions
{
    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--item-name")]
    public string? ItemName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rp-name")]
    public string? RpName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--target-file-share")]
    public string? TargetFileShare { get; set; }

    [CommandSwitch("--target-folder")]
    public string? TargetFolder { get; set; }

    [CommandSwitch("--target-resource-group-name")]
    public string? TargetResourceGroupName { get; set; }

    [CommandSwitch("--target-storage-account")]
    public int? TargetStorageAccount { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }
}