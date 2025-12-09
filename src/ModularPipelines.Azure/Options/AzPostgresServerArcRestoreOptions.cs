using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("postgres", "server-arc", "restore")]
public record AzPostgresServerArcRestoreOptions(
[property: CliOption("--k8s-namespace")] string K8sNamespace,
[property: CliOption("--name")] string Name,
[property: CliOption("--source-server")] string SourceServer
) : AzOptions
{
    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--storage-class-backups")]
    public string? StorageClassBackups { get; set; }

    [CliOption("--storage-class-data")]
    public string? StorageClassData { get; set; }

    [CliOption("--storage-class-logs")]
    public string? StorageClassLogs { get; set; }

    [CliOption("--time")]
    public string? Time { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }

    [CliOption("--volume-size-backups")]
    public string? VolumeSizeBackups { get; set; }

    [CliOption("--volume-size-data")]
    public string? VolumeSizeData { get; set; }

    [CliOption("--volume-size-logs")]
    public string? VolumeSizeLogs { get; set; }
}