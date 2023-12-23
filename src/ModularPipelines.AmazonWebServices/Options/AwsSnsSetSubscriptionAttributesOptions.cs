using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sns", "set-subscription-attributes")]
public record AwsSnsSetSubscriptionAttributesOptions(
[property: CommandSwitch("--subscription-arn")] string SubscriptionArn,
[property: CommandSwitch("--attribute-name")] string AttributeName
) : AwsOptions
{
    [CommandSwitch("--attribute-value")]
    public string? AttributeValue { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}