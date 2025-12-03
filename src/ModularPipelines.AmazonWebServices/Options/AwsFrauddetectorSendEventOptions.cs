using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "send-event")]
public record AwsFrauddetectorSendEventOptions(
[property: CliOption("--event-id")] string EventId,
[property: CliOption("--event-type-name")] string EventTypeName,
[property: CliOption("--event-timestamp")] string EventTimestamp,
[property: CliOption("--event-variables")] IEnumerable<KeyValue> EventVariables,
[property: CliOption("--entities")] string[] Entities
) : AwsOptions
{
    [CliOption("--assigned-label")]
    public string? AssignedLabel { get; set; }

    [CliOption("--label-timestamp")]
    public string? LabelTimestamp { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}