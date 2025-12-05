using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "delete-subscription-definition")]
public record AwsGreengrassDeleteSubscriptionDefinitionOptions(
[property: CliOption("--subscription-definition-id")] string SubscriptionDefinitionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}