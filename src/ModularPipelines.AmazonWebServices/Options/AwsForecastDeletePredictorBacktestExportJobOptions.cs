using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "delete-predictor-backtest-export-job")]
public record AwsForecastDeletePredictorBacktestExportJobOptions(
[property: CommandSwitch("--predictor-backtest-export-job-arn")] string PredictorBacktestExportJobArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}