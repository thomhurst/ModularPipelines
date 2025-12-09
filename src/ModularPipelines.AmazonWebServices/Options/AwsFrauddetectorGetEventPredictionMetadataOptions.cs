using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "get-event-prediction-metadata")]
public record AwsFrauddetectorGetEventPredictionMetadataOptions(
[property: CliOption("--event-id")] string EventId,
[property: CliOption("--event-type-name")] string EventTypeName,
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--detector-version-id")] string DetectorVersionId,
[property: CliOption("--prediction-timestamp")] string PredictionTimestamp
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}