using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "update-pull-request-title")]
public record AwsCodecommitUpdatePullRequestTitleOptions(
[property: CliOption("--pull-request-id")] string PullRequestId,
[property: CliOption("--title")] string Title
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}