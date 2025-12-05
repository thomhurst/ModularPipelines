using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "put-event-type")]
public record AwsFrauddetectorPutEventTypeOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--event-variables")] string[] EventVariables,
[property: CliOption("--entity-types")] string[] EntityTypes
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public string[]? Labels { get; set; }

    [CliOption("--event-ingestion")]
    public string? EventIngestion { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--event-orchestration")]
    public string? EventOrchestration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}