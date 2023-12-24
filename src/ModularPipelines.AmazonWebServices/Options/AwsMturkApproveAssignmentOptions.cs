using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "approve-assignment")]
public record AwsMturkApproveAssignmentOptions(
[property: CommandSwitch("--assignment-id")] string AssignmentId
) : AwsOptions
{
    [CommandSwitch("--requester-feedback")]
    public string? RequesterFeedback { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}