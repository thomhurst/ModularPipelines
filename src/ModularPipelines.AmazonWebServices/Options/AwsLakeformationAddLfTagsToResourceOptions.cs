using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lakeformation", "add-lf-tags-to-resource")]
public record AwsLakeformationAddLfTagsToResourceOptions(
[property: CliOption("--resource")] string Resource,
[property: CliOption("--lf-tags")] string[] LfTags
) : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}