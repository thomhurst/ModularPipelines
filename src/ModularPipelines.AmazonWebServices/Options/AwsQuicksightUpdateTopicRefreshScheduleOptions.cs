using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "update-topic-refresh-schedule")]
public record AwsQuicksightUpdateTopicRefreshScheduleOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--topic-id")] string TopicId,
[property: CommandSwitch("--dataset-id")] string DatasetId,
[property: CommandSwitch("--refresh-schedule")] string RefreshSchedule
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}