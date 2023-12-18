using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "dc", "debug", "restore-controldb-snapshot")]
public record AzArcdataDcDebugRestoreControldbSnapshotOptions(
[property: CommandSwitch("--backup-file")] string BackupFile,
[property: CommandSwitch("--k8s-namespace")] string K8sNamespace
) : AzOptions
{
    [CommandSwitch("--use-k8s")]
    public string? UseK8s { get; set; }
}

