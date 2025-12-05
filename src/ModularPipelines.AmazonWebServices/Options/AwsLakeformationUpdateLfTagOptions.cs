using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lakeformation", "update-lf-tag")]
public record AwsLakeformationUpdateLfTagOptions(
[property: CliOption("--tag-key")] string TagKey
) : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--tag-values-to-delete")]
    public string[]? TagValuesToDelete { get; set; }

    [CliOption("--tag-values-to-add")]
    public string[]? TagValuesToAdd { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}