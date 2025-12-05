using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("xray", "get-trace-summaries")]
public record AwsXrayGetTraceSummariesOptions(
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime
) : AwsOptions
{
    [CliOption("--time-range-type")]
    public string? TimeRangeType { get; set; }

    [CliOption("--sampling-strategy")]
    public string? SamplingStrategy { get; set; }

    [CliOption("--filter-expression")]
    public string? FilterExpression { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}