using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "describe-predictor-backtest-export-job")]
public record AwsForecastDescribePredictorBacktestExportJobOptions(
[property: CommandSwitch("--predictor-backtest-export-job-arn")] string PredictorBacktestExportJobArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}