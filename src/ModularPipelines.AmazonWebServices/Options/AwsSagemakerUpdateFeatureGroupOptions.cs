using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-feature-group")]
public record AwsSagemakerUpdateFeatureGroupOptions(
[property: CliOption("--feature-group-name")] string FeatureGroupName
) : AwsOptions
{
    [CliOption("--feature-additions")]
    public string[]? FeatureAdditions { get; set; }

    [CliOption("--online-store-config")]
    public string? OnlineStoreConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}