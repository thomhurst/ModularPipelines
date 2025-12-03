using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "create-additional-assignments-for-hit")]
public record AwsMturkCreateAdditionalAssignmentsForHitOptions(
[property: CliOption("--hit-id")] string HitId,
[property: CliOption("--number-of-additional-assignments")] int NumberOfAdditionalAssignments
) : AwsOptions
{
    [CliOption("--unique-request-token")]
    public string? UniqueRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}