using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "confirm-topic-rule-destination")]
public record AwsIotConfirmTopicRuleDestinationOptions(
[property: CommandSwitch("--confirmation-token")] string ConfirmationToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}