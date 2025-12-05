using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker-geospatial", "search-raster-data-collection")]
public record AwsSagemakerGeospatialSearchRasterDataCollectionOptions(
[property: CliOption("--arn")] string Arn,
[property: CliOption("--raster-data-collection-query")] string RasterDataCollectionQuery
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}