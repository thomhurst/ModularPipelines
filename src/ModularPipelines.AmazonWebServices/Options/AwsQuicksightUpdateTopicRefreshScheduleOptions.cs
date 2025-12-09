using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-topic-refresh-schedule")]
public record AwsQuicksightUpdateTopicRefreshScheduleOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--topic-id")] string TopicId,
[property: CliOption("--dataset-id")] string DatasetId,
[property: CliOption("--refresh-schedule")] string RefreshSchedule
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}