using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "put-event-type")]
public record AwsFrauddetectorPutEventTypeOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--event-variables")] string[] EventVariables,
[property: CommandSwitch("--entity-types")] string[] EntityTypes
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public string[]? Labels { get; set; }

    [CommandSwitch("--event-ingestion")]
    public string? EventIngestion { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--event-orchestration")]
    public string? EventOrchestration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}