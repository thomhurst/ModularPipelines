using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "create-retraining-scheduler")]
public record AwsLookoutequipmentCreateRetrainingSchedulerOptions(
[property: CliOption("--model-name")] string ModelName,
[property: CliOption("--retraining-frequency")] string RetrainingFrequency,
[property: CliOption("--lookback-window")] string LookbackWindow
) : AwsOptions
{
    [CliOption("--retraining-start-date")]
    public long? RetrainingStartDate { get; set; }

    [CliOption("--promote-mode")]
    public string? PromoteMode { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}