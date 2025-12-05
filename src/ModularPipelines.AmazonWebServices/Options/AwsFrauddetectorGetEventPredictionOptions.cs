using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "get-event-prediction")]
public record AwsFrauddetectorGetEventPredictionOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--event-id")] string EventId,
[property: CliOption("--event-type-name")] string EventTypeName,
[property: CliOption("--entities")] string[] Entities,
[property: CliOption("--event-timestamp")] string EventTimestamp,
[property: CliOption("--event-variables")] IEnumerable<KeyValue> EventVariables
) : AwsOptions
{
    [CliOption("--detector-version-id")]
    public string? DetectorVersionId { get; set; }

    [CliOption("--external-model-endpoint-data-blobs")]
    public IEnumerable<KeyValue>? ExternalModelEndpointDataBlobs { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}