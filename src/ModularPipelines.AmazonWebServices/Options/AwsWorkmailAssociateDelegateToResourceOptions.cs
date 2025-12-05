using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "associate-delegate-to-resource")]
public record AwsWorkmailAssociateDelegateToResourceOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--entity-id")] string EntityId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}