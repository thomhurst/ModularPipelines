using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-feature-group")]
public record AwsSagemakerUpdateFeatureGroupOptions(
[property: CommandSwitch("--feature-group-name")] string FeatureGroupName
) : AwsOptions
{
    [CommandSwitch("--feature-additions")]
    public string[]? FeatureAdditions { get; set; }

    [CommandSwitch("--online-store-config")]
    public string? OnlineStoreConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}