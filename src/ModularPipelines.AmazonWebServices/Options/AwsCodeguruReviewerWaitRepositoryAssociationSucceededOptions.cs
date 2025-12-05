using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguru-reviewer", "wait", "repository-association-succeeded")]
public record AwsCodeguruReviewerWaitRepositoryAssociationSucceededOptions(
[property: CliOption("--association-arn")] string AssociationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}