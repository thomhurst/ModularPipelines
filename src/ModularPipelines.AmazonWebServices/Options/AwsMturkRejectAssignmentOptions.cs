using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "reject-assignment")]
public record AwsMturkRejectAssignmentOptions(
[property: CommandSwitch("--assignment-id")] string AssignmentId,
[property: CommandSwitch("--requester-feedback")] string RequesterFeedback
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}