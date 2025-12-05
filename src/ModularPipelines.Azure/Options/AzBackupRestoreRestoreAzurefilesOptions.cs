using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "restore", "restore-urefiles")]
public record AzBackupRestoreRestoreAzurefilesOptions(
[property: CliOption("--resolve-conflict")] string ResolveConflict,
[property: CliOption("--restore-mode")] string RestoreMode
) : AzOptions
{
    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--item-name")]
    public string? ItemName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rp-name")]
    public string? RpName { get; set; }

    [CliOption("--source-file-path")]
    public string? SourceFilePath { get; set; }

    [CliOption("--source-file-type")]
    public string? SourceFileType { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--target-file-share")]
    public string? TargetFileShare { get; set; }

    [CliOption("--target-folder")]
    public string? TargetFolder { get; set; }

    [CliOption("--target-storage-account")]
    public int? TargetStorageAccount { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}