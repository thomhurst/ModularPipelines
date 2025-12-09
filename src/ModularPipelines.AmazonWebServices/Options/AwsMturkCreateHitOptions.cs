using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "create-hit")]
public record AwsMturkCreateHitOptions(
[property: CliOption("--lifetime-in-seconds")] long LifetimeInSeconds,
[property: CliOption("--assignment-duration-in-seconds")] long AssignmentDurationInSeconds,
[property: CliOption("--reward")] string Reward,
[property: CliOption("--title")] string Title,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--max-assignments")]
    public int? MaxAssignments { get; set; }

    [CliOption("--auto-approval-delay-in-seconds")]
    public long? AutoApprovalDelayInSeconds { get; set; }

    [CliOption("--keywords")]
    public string? Keywords { get; set; }

    [CliOption("--question")]
    public string? Question { get; set; }

    [CliOption("--requester-annotation")]
    public string? RequesterAnnotation { get; set; }

    [CliOption("--qualification-requirements")]
    public string[]? QualificationRequirements { get; set; }

    [CliOption("--unique-request-token")]
    public string? UniqueRequestToken { get; set; }

    [CliOption("--assignment-review-policy")]
    public string? AssignmentReviewPolicy { get; set; }

    [CliOption("--hit-review-policy")]
    public string? HitReviewPolicy { get; set; }

    [CliOption("--hit-layout-id")]
    public string? HitLayoutId { get; set; }

    [CliOption("--hit-layout-parameters")]
    public string[]? HitLayoutParameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}