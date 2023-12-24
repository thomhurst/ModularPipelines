using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "create-forecast")]
public record AwsForecastCreateForecastOptions(
[property: CommandSwitch("--forecast-name")] string ForecastName,
[property: CommandSwitch("--predictor-arn")] string PredictorArn
) : AwsOptions
{
    [CommandSwitch("--forecast-types")]
    public string[]? ForecastTypes { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--time-series-selector")]
    public string? TimeSeriesSelector { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}