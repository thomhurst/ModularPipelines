using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sns", "set-topic-attributes")]
public record AwsSnsSetTopicAttributesOptions(
[property: CommandSwitch("--topic-arn")] string TopicArn,
[property: CommandSwitch("--attribute-name")] string AttributeName
) : AwsOptions
{
    [CommandSwitch("--attribute-value")]
    public string? AttributeValue { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}