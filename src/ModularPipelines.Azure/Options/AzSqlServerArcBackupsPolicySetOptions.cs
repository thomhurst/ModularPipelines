using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server-arc", "backups-policy", "set")]
public record AzSqlServerArcBackupsPolicySetOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--default-policy")]
    public bool? DefaultPolicy { get; set; }

    [CommandSwitch("--diff-backup-hours")]
    public string? DiffBackupHours { get; set; }

    [CommandSwitch("--full-backup-days")]
    public string? FullBackupDays { get; set; }

    [CommandSwitch("--retention-days")]
    public string? RetentionDays { get; set; }

    [CommandSwitch("--tlog-backup-mins")]
    public string? TlogBackupMins { get; set; }
}