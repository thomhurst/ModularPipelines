using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "replace-topic-rule")]
public record AwsIotReplaceTopicRuleOptions(
[property: CliOption("--rule-name")] string RuleName,
[property: CliOption("--topic-rule-payload")] string TopicRulePayload
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}