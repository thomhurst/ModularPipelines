using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "server-arc", "backups-policy", "set")]
public record AzSqlServerArcBackupsPolicySetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--default-policy")]
    public bool? DefaultPolicy { get; set; }

    [CliOption("--diff-backup-hours")]
    public string? DiffBackupHours { get; set; }

    [CliOption("--full-backup-days")]
    public string? FullBackupDays { get; set; }

    [CliOption("--retention-days")]
    public string? RetentionDays { get; set; }

    [CliOption("--tlog-backup-mins")]
    public string? TlogBackupMins { get; set; }
}