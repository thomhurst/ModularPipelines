using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "create-predictor-backtest-export-job")]
public record AwsForecastCreatePredictorBacktestExportJobOptions(
[property: CommandSwitch("--predictor-backtest-export-job-name")] string PredictorBacktestExportJobName,
[property: CommandSwitch("--predictor-arn")] string PredictorArn,
[property: CommandSwitch("--destination")] string Destination
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}