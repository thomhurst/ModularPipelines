using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworkscm", "update-server")]
public record AwsOpsworkscmUpdateServerOptions(
[property: CommandSwitch("--server-name")] string ServerName
) : AwsOptions
{
    [CommandSwitch("--backup-retention-count")]
    public int? BackupRetentionCount { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}