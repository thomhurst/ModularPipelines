using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker-geospatial", "search-raster-data-collection")]
public record AwsSagemakerGeospatialSearchRasterDataCollectionOptions(
[property: CommandSwitch("--arn")] string Arn,
[property: CommandSwitch("--raster-data-collection-query")] string RasterDataCollectionQuery
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}