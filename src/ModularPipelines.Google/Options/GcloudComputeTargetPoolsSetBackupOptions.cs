using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-pools", "set-backup")]
public record GcloudComputeTargetPoolsSetBackupOptions(
[property: CliArgument] string Name,
[property: CliOption("--backup-pool")] string BackupPool,
[property: CliFlag("--no-backup-pool")] bool NoBackupPool
) : GcloudOptions
{
    [CliOption("--failover-ratio")]
    public string? FailoverRatio { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}