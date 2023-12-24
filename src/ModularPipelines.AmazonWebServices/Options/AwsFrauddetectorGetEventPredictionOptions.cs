using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "get-event-prediction")]
public record AwsFrauddetectorGetEventPredictionOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--event-id")] string EventId,
[property: CommandSwitch("--event-type-name")] string EventTypeName,
[property: CommandSwitch("--entities")] string[] Entities,
[property: CommandSwitch("--event-timestamp")] string EventTimestamp,
[property: CommandSwitch("--event-variables")] IEnumerable<KeyValue> EventVariables
) : AwsOptions
{
    [CommandSwitch("--detector-version-id")]
    public string? DetectorVersionId { get; set; }

    [CommandSwitch("--external-model-endpoint-data-blobs")]
    public IEnumerable<KeyValue>? ExternalModelEndpointDataBlobs { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}