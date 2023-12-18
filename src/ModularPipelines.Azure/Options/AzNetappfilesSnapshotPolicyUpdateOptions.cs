using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "snapshot", "policy", "update")]
public record AzNetappfilesSnapshotPolicyUpdateOptions : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--daily-hour")]
    public string? DailyHour { get; set; }

    [CommandSwitch("--daily-minute")]
    public string? DailyMinute { get; set; }

    [CommandSwitch("--daily-snapshots")]
    public string? DailySnapshots { get; set; }

    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--hourly-minute")]
    public string? HourlyMinute { get; set; }

    [CommandSwitch("--hourly-snapshots")]
    public string? HourlySnapshots { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--monthly-days")]
    public string? MonthlyDays { get; set; }

    [CommandSwitch("--monthly-hour")]
    public string? MonthlyHour { get; set; }

    [CommandSwitch("--monthly-minute")]
    public string? MonthlyMinute { get; set; }

    [CommandSwitch("--monthly-snapshots")]
    public string? MonthlySnapshots { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--snapshot-policy-name")]
    public string? SnapshotPolicyName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--weekly-day")]
    public string? WeeklyDay { get; set; }

    [CommandSwitch("--weekly-hour")]
    public string? WeeklyHour { get; set; }

    [CommandSwitch("--weekly-minute")]
    public string? WeeklyMinute { get; set; }

    [CommandSwitch("--weekly-snapshots")]
    public string? WeeklySnapshots { get; set; }
}