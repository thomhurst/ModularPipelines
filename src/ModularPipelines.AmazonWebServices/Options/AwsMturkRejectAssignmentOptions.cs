using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "reject-assignment")]
public record AwsMturkRejectAssignmentOptions(
[property: CliOption("--assignment-id")] string AssignmentId,
[property: CliOption("--requester-feedback")] string RequesterFeedback
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}