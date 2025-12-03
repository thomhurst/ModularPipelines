using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "create-forecast")]
public record AwsForecastCreateForecastOptions(
[property: CliOption("--forecast-name")] string ForecastName,
[property: CliOption("--predictor-arn")] string PredictorArn
) : AwsOptions
{
    [CliOption("--forecast-types")]
    public string[]? ForecastTypes { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--time-series-selector")]
    public string? TimeSeriesSelector { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}