using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "replace-topic-rule")]
public record AwsIotReplaceTopicRuleOptions(
[property: CommandSwitch("--rule-name")] string RuleName,
[property: CommandSwitch("--topic-rule-payload")] string TopicRulePayload
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}