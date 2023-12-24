using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "schedule-hbase-backup")]
public record AwsEmrScheduleHbaseBackupOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--dir")] string Dir,
[property: CommandSwitch("--interval")] string Interval,
[property: CommandSwitch("--unit")] string Unit
) : AwsOptions
{
    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [BooleanCommandSwitch("--consistent")]
    public bool? Consistent { get; set; }
}