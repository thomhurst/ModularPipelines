using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "schedule-hbase-backup")]
public record AwsEmrScheduleHbaseBackupOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--type")] string Type,
[property: CliOption("--dir")] string Dir,
[property: CliOption("--interval")] string Interval,
[property: CliOption("--unit")] string Unit
) : AwsOptions
{
    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliFlag("--consistent")]
    public bool? Consistent { get; set; }
}