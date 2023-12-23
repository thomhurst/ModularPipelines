using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "create-retraining-scheduler")]
public record AwsLookoutequipmentCreateRetrainingSchedulerOptions(
[property: CommandSwitch("--model-name")] string ModelName,
[property: CommandSwitch("--retraining-frequency")] string RetrainingFrequency,
[property: CommandSwitch("--lookback-window")] string LookbackWindow
) : AwsOptions
{
    [CommandSwitch("--retraining-start-date")]
    public long? RetrainingStartDate { get; set; }

    [CommandSwitch("--promote-mode")]
    public string? PromoteMode { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}