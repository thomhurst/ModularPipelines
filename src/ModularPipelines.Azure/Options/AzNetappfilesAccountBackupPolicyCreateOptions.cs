using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles", "account", "backup-policy", "create")]
public record AzNetappfilesAccountBackupPolicyCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--backup-policy-name")] string BackupPolicyName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--daily-backups")]
    public string? DailyBackups { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--monthly-backups")]
    public string? MonthlyBackups { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--weekly-backups")]
    public string? WeeklyBackups { get; set; }
}