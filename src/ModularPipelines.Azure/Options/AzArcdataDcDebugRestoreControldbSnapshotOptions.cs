using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcdata", "dc", "debug", "restore-controldb-snapshot")]
public record AzArcdataDcDebugRestoreControldbSnapshotOptions(
[property: CliOption("--backup-file")] string BackupFile,
[property: CliOption("--k8s-namespace")] string K8sNamespace
) : AzOptions
{
    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}