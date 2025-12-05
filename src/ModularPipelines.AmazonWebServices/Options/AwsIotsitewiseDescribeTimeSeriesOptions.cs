using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "describe-time-series")]
public record AwsIotsitewiseDescribeTimeSeriesOptions : AwsOptions
{
    [CliOption("--alias")]
    public string? Alias { get; set; }

    [CliOption("--asset-id")]
    public string? AssetId { get; set; }

    [CliOption("--property-id")]
    public string? PropertyId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}