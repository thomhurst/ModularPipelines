using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "list-time-series")]
public record AwsIotsitewiseListTimeSeriesOptions : AwsOptions
{
    [CliOption("--asset-id")]
    public string? AssetId { get; set; }

    [CliOption("--alias-prefix")]
    public string? AliasPrefix { get; set; }

    [CliOption("--time-series-type")]
    public string? TimeSeriesType { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}