using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles", "snapshot", "policy", "create")]
public record AzNetappfilesSnapshotPolicyCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--snapshot-policy-name")] string SnapshotPolicyName
) : AzOptions
{
    [CliOption("--daily-hour")]
    public string? DailyHour { get; set; }

    [CliOption("--daily-minute")]
    public string? DailyMinute { get; set; }

    [CliOption("--daily-snapshots")]
    public string? DailySnapshots { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--hourly-minute")]
    public string? HourlyMinute { get; set; }

    [CliOption("--hourly-snapshots")]
    public string? HourlySnapshots { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--monthly-days")]
    public string? MonthlyDays { get; set; }

    [CliOption("--monthly-hour")]
    public string? MonthlyHour { get; set; }

    [CliOption("--monthly-minute")]
    public string? MonthlyMinute { get; set; }

    [CliOption("--monthly-snapshots")]
    public string? MonthlySnapshots { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--weekly-day")]
    public string? WeeklyDay { get; set; }

    [CliOption("--weekly-hour")]
    public string? WeeklyHour { get; set; }

    [CliOption("--weekly-minute")]
    public string? WeeklyMinute { get; set; }

    [CliOption("--weekly-snapshots")]
    public string? WeeklySnapshots { get; set; }
}