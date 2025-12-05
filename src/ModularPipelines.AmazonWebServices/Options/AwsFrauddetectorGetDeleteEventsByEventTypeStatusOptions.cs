using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "get-delete-events-by-event-type-status")]
public record AwsFrauddetectorGetDeleteEventsByEventTypeStatusOptions(
[property: CliOption("--event-type-name")] string EventTypeName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}