using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "get-buckets-aggregation")]
public record AwsIotGetBucketsAggregationOptions(
[property: CliOption("--query-string")] string QueryString,
[property: CliOption("--aggregation-field")] string AggregationField,
[property: CliOption("--buckets-aggregation-type")] string BucketsAggregationType
) : AwsOptions
{
    [CliOption("--index-name")]
    public string? IndexName { get; set; }

    [CliOption("--query-version")]
    public string? QueryVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}