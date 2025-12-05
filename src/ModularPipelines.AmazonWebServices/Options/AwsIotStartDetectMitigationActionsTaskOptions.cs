using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "start-detect-mitigation-actions-task")]
public record AwsIotStartDetectMitigationActionsTaskOptions(
[property: CliOption("--task-id")] string TaskId,
[property: CliOption("--target")] string Target,
[property: CliOption("--actions")] string[] Actions
) : AwsOptions
{
    [CliOption("--violation-event-occurrence-range")]
    public string? ViolationEventOccurrenceRange { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}