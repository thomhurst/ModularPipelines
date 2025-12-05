using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "delete-topic-refresh-schedule")]
public record AwsQuicksightDeleteTopicRefreshScheduleOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--topic-id")] string TopicId,
[property: CliOption("--dataset-id")] string DatasetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}