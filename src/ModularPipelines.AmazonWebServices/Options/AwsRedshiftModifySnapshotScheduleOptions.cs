using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "modify-snapshot-schedule")]
public record AwsRedshiftModifySnapshotScheduleOptions(
[property: CommandSwitch("--schedule-identifier")] string ScheduleIdentifier,
[property: CommandSwitch("--schedule-definitions")] string[] ScheduleDefinitions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}