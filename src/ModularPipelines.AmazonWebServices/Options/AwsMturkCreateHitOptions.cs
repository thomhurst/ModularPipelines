using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "create-hit")]
public record AwsMturkCreateHitOptions(
[property: CommandSwitch("--lifetime-in-seconds")] long LifetimeInSeconds,
[property: CommandSwitch("--assignment-duration-in-seconds")] long AssignmentDurationInSeconds,
[property: CommandSwitch("--reward")] string Reward,
[property: CommandSwitch("--title")] string Title,
[property: CommandSwitch("--description")] string Description
) : AwsOptions
{
    [CommandSwitch("--max-assignments")]
    public int? MaxAssignments { get; set; }

    [CommandSwitch("--auto-approval-delay-in-seconds")]
    public long? AutoApprovalDelayInSeconds { get; set; }

    [CommandSwitch("--keywords")]
    public string? Keywords { get; set; }

    [CommandSwitch("--question")]
    public string? Question { get; set; }

    [CommandSwitch("--requester-annotation")]
    public string? RequesterAnnotation { get; set; }

    [CommandSwitch("--qualification-requirements")]
    public string[]? QualificationRequirements { get; set; }

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