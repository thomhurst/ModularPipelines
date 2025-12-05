using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "get-subscription-definition")]
public record AwsGreengrassGetSubscriptionDefinitionOptions(
[property: CliOption("--subscription-definition-id")] string SubscriptionDefinitionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}