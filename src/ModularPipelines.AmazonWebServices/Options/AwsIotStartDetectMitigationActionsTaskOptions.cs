using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "start-detect-mitigation-actions-task")]
public record AwsIotStartDetectMitigationActionsTaskOptions(
[property: CommandSwitch("--task-id")] string TaskId,
[property: CommandSwitch("--target")] string Target,
[property: CommandSwitch("--actions")] string[] Actions
) : AwsOptions
{
    [CommandSwitch("--violation-event-occurrence-range")]
    public string? ViolationEventOccurrenceRange { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}