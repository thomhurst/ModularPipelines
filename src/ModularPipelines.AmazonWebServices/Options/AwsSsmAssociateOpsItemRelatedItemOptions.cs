using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "associate-ops-item-related-item")]
public record AwsSsmAssociateOpsItemRelatedItemOptions(
[property: CliOption("--ops-item-id")] string OpsItemId,
[property: CliOption("--association-type")] string AssociationType,
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--resource-uri")] string ResourceUri
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}