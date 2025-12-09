using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-topic-rule")]
public record AwsIotCreateTopicRuleOptions(
[property: CliOption("--rule-name")] string RuleName,
[property: CliOption("--topic-rule-payload")] string TopicRulePayload
) : AwsOptions
{
    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}