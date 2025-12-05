using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "update-event-label")]
public record AwsFrauddetectorUpdateEventLabelOptions(
[property: CliOption("--event-id")] string EventId,
[property: CliOption("--event-type-name")] string EventTypeName,
[property: CliOption("--assigned-label")] string AssignedLabel,
[property: CliOption("--label-timestamp")] string LabelTimestamp
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}