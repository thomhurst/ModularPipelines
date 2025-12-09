using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "subscribe-to-event")]
public record AwsInspectorSubscribeToEventOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--event")] string Event,
[property: CliOption("--topic-arn")] string TopicArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}