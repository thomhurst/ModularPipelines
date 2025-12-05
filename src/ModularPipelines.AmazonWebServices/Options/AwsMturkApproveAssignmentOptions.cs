using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "approve-assignment")]
public record AwsMturkApproveAssignmentOptions(
[property: CliOption("--assignment-id")] string AssignmentId
) : AwsOptions
{
    [CliOption("--requester-feedback")]
    public string? RequesterFeedback { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}