using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "create-additional-assignments-for-hit")]
public record AwsMturkCreateAdditionalAssignmentsForHitOptions(
[property: CommandSwitch("--hit-id")] string HitId,
[property: CommandSwitch("--number-of-additional-assignments")] int NumberOfAdditionalAssignments
) : AwsOptions
{
    [CommandSwitch("--unique-request-token")]
    public string? UniqueRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}