using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sns", "publish-batch")]
public record AwsSnsPublishBatchOptions(
[property: CommandSwitch("--topic-arn")] string TopicArn,
[property: CommandSwitch("--publish-batch-request-entries")] string[] PublishBatchRequestEntries
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}