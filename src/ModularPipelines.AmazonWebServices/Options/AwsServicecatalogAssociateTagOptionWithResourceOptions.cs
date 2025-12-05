using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "associate-tag-option-with-resource")]
public record AwsServicecatalogAssociateTagOptionWithResourceOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--tag-option-id")] string TagOptionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}