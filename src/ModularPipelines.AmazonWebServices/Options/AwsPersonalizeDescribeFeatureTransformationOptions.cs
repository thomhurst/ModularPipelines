using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "describe-feature-transformation")]
public record AwsPersonalizeDescribeFeatureTransformationOptions(
[property: CliOption("--feature-transformation-arn")] string FeatureTransformationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}