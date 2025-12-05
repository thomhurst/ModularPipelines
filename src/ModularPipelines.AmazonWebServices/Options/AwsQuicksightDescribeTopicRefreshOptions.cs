using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "describe-topic-refresh")]
public record AwsQuicksightDescribeTopicRefreshOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--topic-id")] string TopicId,
[property: CliOption("--refresh-id")] string RefreshId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}