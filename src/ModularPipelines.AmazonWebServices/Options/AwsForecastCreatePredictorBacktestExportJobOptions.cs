using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "create-predictor-backtest-export-job")]
public record AwsForecastCreatePredictorBacktestExportJobOptions(
[property: CliOption("--predictor-backtest-export-job-name")] string PredictorBacktestExportJobName,
[property: CliOption("--predictor-arn")] string PredictorArn,
[property: CliOption("--destination")] string Destination
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--format")]
    public string? Format { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}