using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "get-subscription-definition")]
public record AwsGreengrassGetSubscriptionDefinitionOptions(
[property: CommandSwitch("--subscription-definition-id")] string SubscriptionDefinitionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}