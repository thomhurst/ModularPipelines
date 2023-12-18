using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "snapshot", "policy", "create")]
public record AzNetappfilesSnapshotPolicyCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--snapshot-policy-name")] string SnapshotPolicyName
) : AzOptions
{
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

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--monthly-days")]
    public string? MonthlyDays { get; set; }

    [CommandSwitch("--monthly-hour")]
    public string? MonthlyHour { get; set; }

    [CommandSwitch("--monthly-minute")]
    public string? MonthlyMinute { get; set; }

    [CommandSwitch("--monthly-snapshots")]
    public string? MonthlySnapshots { get; set; }

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