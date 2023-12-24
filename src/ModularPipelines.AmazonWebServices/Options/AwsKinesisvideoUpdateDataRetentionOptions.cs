using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisvideo", "update-data-retention")]
public record AwsKinesisvideoUpdateDataRetentionOptions(
[property: CommandSwitch("--current-version")] string CurrentVersion,
[property: CommandSwitch("--operation")] string Operation,
[property: CommandSwitch("--data-retention-change-in-hours")] int DataRetentionChangeInHours
) : AwsOptions
{
    [CommandSwitch("--stream-name")]
    public string? StreamName { get; set; }

    [CommandSwitch("--stream-arn")]
    public string? StreamArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}