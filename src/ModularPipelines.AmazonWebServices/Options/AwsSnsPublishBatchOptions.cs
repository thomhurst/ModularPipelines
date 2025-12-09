using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sns", "publish-batch")]
public record AwsSnsPublishBatchOptions(
[property: CliOption("--topic-arn")] string TopicArn,
[property: CliOption("--publish-batch-request-entries")] string[] PublishBatchRequestEntries
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}