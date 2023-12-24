using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "get-event-prediction-metadata")]
public record AwsFrauddetectorGetEventPredictionMetadataOptions(
[property: CommandSwitch("--event-id")] string EventId,
[property: CommandSwitch("--event-type-name")] string EventTypeName,
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--detector-version-id")] string DetectorVersionId,
[property: CommandSwitch("--prediction-timestamp")] string PredictionTimestamp
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}