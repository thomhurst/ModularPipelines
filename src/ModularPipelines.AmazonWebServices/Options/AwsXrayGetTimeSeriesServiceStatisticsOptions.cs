using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("xray", "get-time-series-service-statistics")]
public record AwsXrayGetTimeSeriesServiceStatisticsOptions(
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime
) : AwsOptions
{
    [CliOption("--group-name")]
    public string? GroupName { get; set; }

    [CliOption("--group-arn")]
    public string? GroupArn { get; set; }

    [CliOption("--entity-selector-expression")]
    public string? EntitySelectorExpression { get; set; }

    [CliOption("--period")]
    public int? Period { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}