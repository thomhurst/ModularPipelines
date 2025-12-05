using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "create-hit-type")]
public record AwsMturkCreateHitTypeOptions(
[property: CliOption("--assignment-duration-in-seconds")] long AssignmentDurationInSeconds,
[property: CliOption("--reward")] string Reward,
[property: CliOption("--title")] string Title,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--auto-approval-delay-in-seconds")]
    public long? AutoApprovalDelayInSeconds { get; set; }

    [CliOption("--keywords")]
    public string? Keywords { get; set; }

    [CliOption("--qualification-requirements")]
    public string[]? QualificationRequirements { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}