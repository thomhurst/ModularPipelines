using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-topic-rule")]
public record AwsIotCreateTopicRuleOptions(
[property: CommandSwitch("--rule-name")] string RuleName,
[property: CommandSwitch("--topic-rule-payload")] string TopicRulePayload
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}