using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "get-statistics")]
public record AwsIotGetStatisticsOptions(
[property: CliOption("--query-string")] string QueryString
) : AwsOptions
{
    [CliOption("--index-name")]
    public string? IndexName { get; set; }

    [CliOption("--aggregation-field")]
    public string? AggregationField { get; set; }

    [CliOption("--query-version")]
    public string? QueryVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}