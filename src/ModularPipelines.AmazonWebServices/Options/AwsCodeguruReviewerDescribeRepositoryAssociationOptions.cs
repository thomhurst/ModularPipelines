using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguru-reviewer", "describe-repository-association")]
public record AwsCodeguruReviewerDescribeRepositoryAssociationOptions(
[property: CliOption("--association-arn")] string AssociationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}