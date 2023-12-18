using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "dc", "debug", "restore-controldb-snapshot")]
public record AzArcdataDcDebugRestoreControldbSnapshotOptions(
[property: CommandSwitch("--backup-file")] string BackupFile,
[property: CommandSwitch("--k8s-namespace")] string K8sNamespace
) : AzOptions
{
    [BooleanCommandSwitch("--use-k8s")]
    public bool? UseK8s { get; set; }
}