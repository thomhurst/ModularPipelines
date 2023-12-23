using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "update-snapshot-schedule")]
public record AwsStoragegatewayUpdateSnapshotScheduleOptions(
[property: CommandSwitch("--volume-arn")] string VolumeArn,
[property: CommandSwitch("--start-at")] int StartAt,
[property: CommandSwitch("--recurrence-in-hours")] int RecurrenceInHours
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}