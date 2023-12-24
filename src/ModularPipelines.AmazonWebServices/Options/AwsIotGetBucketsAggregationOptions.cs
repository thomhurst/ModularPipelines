using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "get-buckets-aggregation")]
public record AwsIotGetBucketsAggregationOptions(
[property: CommandSwitch("--query-string")] string QueryString,
[property: CommandSwitch("--aggregation-field")] string AggregationField,
[property: CommandSwitch("--buckets-aggregation-type")] string BucketsAggregationType
) : AwsOptions
{
    [CommandSwitch("--index-name")]
    public string? IndexName { get; set; }

    [CommandSwitch("--query-version")]
    public string? QueryVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}