using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "describe-feature-metadata")]
public record AwsSagemakerDescribeFeatureMetadataOptions(
[property: CommandSwitch("--feature-group-name")] string FeatureGroupName,
[property: CommandSwitch("--feature-name")] string FeatureName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}