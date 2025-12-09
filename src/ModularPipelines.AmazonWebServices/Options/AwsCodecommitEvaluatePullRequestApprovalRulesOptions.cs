using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "evaluate-pull-request-approval-rules")]
public record AwsCodecommitEvaluatePullRequestApprovalRulesOptions(
[property: CliOption("--pull-request-id")] string PullRequestId,
[property: CliOption("--revision-id")] string RevisionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}