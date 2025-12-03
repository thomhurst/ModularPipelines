using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguru-reviewer", "create-code-review")]
public record AwsCodeguruReviewerCreateCodeReviewOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--repository-association-arn")] string RepositoryAssociationArn,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}