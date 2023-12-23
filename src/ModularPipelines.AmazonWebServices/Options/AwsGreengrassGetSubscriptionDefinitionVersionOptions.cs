using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "get-subscription-definition-version")]
public record AwsGreengrassGetSubscriptionDefinitionVersionOptions(
[property: CommandSwitch("--subscription-definition-id")] string SubscriptionDefinitionId,
[property: CommandSwitch("--subscription-definition-version-id")] string SubscriptionDefinitionVersionId
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}