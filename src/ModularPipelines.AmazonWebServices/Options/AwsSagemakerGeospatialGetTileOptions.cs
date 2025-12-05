using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker-geospatial", "get-tile")]
public record AwsSagemakerGeospatialGetTileOptions(
[property: CliOption("--arn")] string Arn,
[property: CliOption("--image-assets")] string[] ImageAssets,
[property: CliOption("--target")] string Target,
[property: CliOption("--x")] int X,
[property: CliOption("--y")] int Y,
[property: CliOption("--z")] int Z
) : AwsOptions
{
    [CliOption("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CliOption("--output-data-type")]
    public string? OutputDataType { get; set; }

    [CliOption("--output-format")]
    public string? OutputFormat { get; set; }

    [CliOption("--property-filters")]
    public string? PropertyFilters { get; set; }

    [CliOption("--time-range-filter")]
    public string? TimeRangeFilter { get; set; }
}