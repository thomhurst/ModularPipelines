using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "account", "backup-policy", "update")]
public record AzNetappfilesAccountBackupPolicyUpdateOptions : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--backup-policy-name")]
    public string? BackupPolicyName { get; set; }

    [CliOption("--daily-backups")]
    public string? DailyBackups { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--monthly-backups")]
    public string? MonthlyBackups { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--weekly-backups")]
    public string? WeeklyBackups { get; set; }
}