using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "update-retraining-scheduler")]
public record AwsLookoutequipmentUpdateRetrainingSchedulerOptions(
[property: CliOption("--model-name")] string ModelName
) : AwsOptions
{
    [CliOption("--retraining-start-date")]
    public long? RetrainingStartDate { get; set; }

    [CliOption("--retraining-frequency")]
    public string? RetrainingFrequency { get; set; }

    [CliOption("--lookback-window")]
    public string? LookbackWindow { get; set; }

    [CliOption("--promote-mode")]
    public string? PromoteMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}