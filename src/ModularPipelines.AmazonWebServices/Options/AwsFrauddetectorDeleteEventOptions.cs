using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "delete-event")]
public record AwsFrauddetectorDeleteEventOptions(
[property: CliOption("--event-id")] string EventId,
[property: CliOption("--event-type-name")] string EventTypeName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}