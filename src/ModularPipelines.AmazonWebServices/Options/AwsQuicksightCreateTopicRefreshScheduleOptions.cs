using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "create-topic-refresh-schedule")]
public record AwsQuicksightCreateTopicRefreshScheduleOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--topic-id")] string TopicId,
[property: CommandSwitch("--dataset-arn")] string DatasetArn,
[property: CommandSwitch("--refresh-schedule")] string RefreshSchedule
) : AwsOptions
{
    [CommandSwitch("--dataset-name")]
    public string? DatasetName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}