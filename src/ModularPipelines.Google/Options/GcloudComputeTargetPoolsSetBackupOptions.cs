using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-pools", "set-backup")]
public record GcloudComputeTargetPoolsSetBackupOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--backup-pool")] string BackupPool,
[property: BooleanCommandSwitch("--no-backup-pool")] bool NoBackupPool
) : GcloudOptions
{
    [CommandSwitch("--failover-ratio")]
    public string? FailoverRatio { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}