using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "create-hit-type")]
public record AwsMturkCreateHitTypeOptions(
[property: CommandSwitch("--assignment-duration-in-seconds")] long AssignmentDurationInSeconds,
[property: CommandSwitch("--reward")] string Reward,
[property: CommandSwitch("--title")] string Title,
[property: CommandSwitch("--description")] string Description
) : AwsOptions
{
    [CommandSwitch("--auto-approval-delay-in-seconds")]
    public long? AutoApprovalDelayInSeconds { get; set; }

    [CommandSwitch("--keywords")]
    public string? Keywords { get; set; }

    [CommandSwitch("--qualification-requirements")]
    public string[]? QualificationRequirements { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}