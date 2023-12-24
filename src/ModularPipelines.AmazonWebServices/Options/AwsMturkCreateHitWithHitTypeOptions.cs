using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "create-hit-with-hit-type")]
public record AwsMturkCreateHitWithHitTypeOptions(
[property: CommandSwitch("--hit-type-id")] string HitTypeId,
[property: CommandSwitch("--lifetime-in-seconds")] long LifetimeInSeconds
) : AwsOptions
{
    [CommandSwitch("--max-assignments")]
    public int? MaxAssignments { get; set; }

    [CommandSwitch("--question")]
    public string? Question { get; set; }

    [CommandSwitch("--requester-annotation")]
    public string? RequesterAnnotation { get; set; }

    [CommandSwitch("--unique-request-token")]
    public string? UniqueRequestToken { get; set; }

    [CommandSwitch("--assignment-review-policy")]
    public string? AssignmentReviewPolicy { get; set; }

    [CommandSwitch("--hit-review-policy")]
    public string? HitReviewPolicy { get; set; }

    [CommandSwitch("--hit-layout-id")]
    public string? HitLayoutId { get; set; }

    [CommandSwitch("--hit-layout-parameters")]
    public string[]? HitLayoutParameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}