using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "get-subscription-definition-version")]
public record AwsGreengrassGetSubscriptionDefinitionVersionOptions(
[property: CliOption("--subscription-definition-id")] string SubscriptionDefinitionId,
[property: CliOption("--subscription-definition-version-id")] string SubscriptionDefinitionVersionId
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}