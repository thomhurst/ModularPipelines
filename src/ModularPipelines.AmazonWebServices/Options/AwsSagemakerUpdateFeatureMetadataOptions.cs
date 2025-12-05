using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-feature-metadata")]
public record AwsSagemakerUpdateFeatureMetadataOptions(
[property: CliOption("--feature-group-name")] string FeatureGroupName,
[property: CliOption("--feature-name")] string FeatureName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--parameter-additions")]
    public string[]? ParameterAdditions { get; set; }

    [CliOption("--parameter-removals")]
    public string[]? ParameterRemovals { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}