using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres", "server-arc", "restore")]
public record AzPostgresServerArcRestoreOptions(
[property: CommandSwitch("--k8s-namespace")] string K8sNamespace,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--source-server")] string SourceServer
) : AzOptions
{
    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--storage-class-backups")]
    public string? StorageClassBackups { get; set; }

    [CommandSwitch("--storage-class-data")]
    public string? StorageClassData { get; set; }

    [CommandSwitch("--storage-class-logs")]
    public string? StorageClassLogs { get; set; }

    [CommandSwitch("--time")]
    public string? Time { get; set; }

    [CommandSwitch("--use-k8s")]
    public string? UseK8s { get; set; }

    [CommandSwitch("--volume-size-backups")]
    public string? VolumeSizeBackups { get; set; }

    [CommandSwitch("--volume-size-data")]
    public string? VolumeSizeData { get; set; }

    [CommandSwitch("--volume-size-logs")]
    public string? VolumeSizeLogs { get; set; }
}