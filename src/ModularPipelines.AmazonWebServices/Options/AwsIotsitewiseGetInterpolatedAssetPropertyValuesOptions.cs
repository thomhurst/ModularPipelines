using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "get-interpolated-asset-property-values")]
public record AwsIotsitewiseGetInterpolatedAssetPropertyValuesOptions(
[property: CliOption("--start-time-in-seconds")] long StartTimeInSeconds,
[property: CliOption("--end-time-in-seconds")] long EndTimeInSeconds,
[property: CliOption("--quality")] string Quality,
[property: CliOption("--interval-in-seconds")] long IntervalInSeconds,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--asset-id")]
    public string? AssetId { get; set; }

    [CliOption("--property-id")]
    public string? PropertyId { get; set; }

    [CliOption("--property-alias")]
    public string? PropertyAlias { get; set; }

    [CliOption("--start-time-offset-in-nanos")]
    public int? StartTimeOffsetInNanos { get; set; }

    [CliOption("--end-time-offset-in-nanos")]
    public int? EndTimeOffsetInNanos { get; set; }

    [CliOption("--interval-window-in-seconds")]
    public long? IntervalWindowInSeconds { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}