using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "update-retraining-scheduler")]
public record AwsLookoutequipmentUpdateRetrainingSchedulerOptions(
[property: CommandSwitch("--model-name")] string ModelName
) : AwsOptions
{
    [CommandSwitch("--retraining-start-date")]
    public long? RetrainingStartDate { get; set; }

    [CommandSwitch("--retraining-frequency")]
    public string? RetrainingFrequency { get; set; }

    [CommandSwitch("--lookback-window")]
    public string? LookbackWindow { get; set; }

    [CommandSwitch("--promote-mode")]
    public string? PromoteMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}