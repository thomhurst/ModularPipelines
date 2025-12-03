using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "disassociate-delegate-from-resource")]
public record AwsWorkmailDisassociateDelegateFromResourceOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--entity-id")] string EntityId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}