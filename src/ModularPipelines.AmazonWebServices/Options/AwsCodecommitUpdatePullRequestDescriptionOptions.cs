using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "update-pull-request-description")]
public record AwsCodecommitUpdatePullRequestDescriptionOptions(
[property: CliOption("--pull-request-id")] string PullRequestId,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}