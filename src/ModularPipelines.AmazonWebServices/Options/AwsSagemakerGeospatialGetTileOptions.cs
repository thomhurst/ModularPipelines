using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker-geospatial", "get-tile")]
public record AwsSagemakerGeospatialGetTileOptions(
[property: CommandSwitch("--arn")] string Arn,
[property: CommandSwitch("--image-assets")] string[] ImageAssets,
[property: CommandSwitch("--target")] string Target,
[property: CommandSwitch("--x")] int X,
[property: CommandSwitch("--y")] int Y,
[property: CommandSwitch("--z")] int Z
) : AwsOptions
{
    [CommandSwitch("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CommandSwitch("--output-data-type")]
    public string? OutputDataType { get; set; }

    [CommandSwitch("--output-format")]
    public string? OutputFormat { get; set; }

    [CommandSwitch("--property-filters")]
    public string? PropertyFilters { get; set; }

    [CommandSwitch("--time-range-filter")]
    public string? TimeRangeFilter { get; set; }
}