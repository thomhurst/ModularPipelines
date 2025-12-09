using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-feature-metadata")]
public record AwsSagemakerDescribeFeatureMetadataOptions(
[property: CliOption("--feature-group-name")] string FeatureGroupName,
[property: CliOption("--feature-name")] string FeatureName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}