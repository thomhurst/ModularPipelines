using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "delete-predictor-backtest-export-job")]
public record AwsForecastDeletePredictorBacktestExportJobOptions(
[property: CliOption("--predictor-backtest-export-job-arn")] string PredictorBacktestExportJobArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}