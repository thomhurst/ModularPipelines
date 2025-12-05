using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "create-topic-refresh-schedule")]
public record AwsQuicksightCreateTopicRefreshScheduleOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--topic-id")] string TopicId,
[property: CliOption("--dataset-arn")] string DatasetArn,
[property: CliOption("--refresh-schedule")] string RefreshSchedule
) : AwsOptions
{
    [CliOption("--dataset-name")]
    public string? DatasetName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}