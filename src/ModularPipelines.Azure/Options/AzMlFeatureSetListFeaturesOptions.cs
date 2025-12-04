using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "feature-set", "list-features")]
public record AzMlFeatureSetListFeaturesOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--version")] string Version
) : AzOptions
{
    [CliOption("--feature-name")]
    public string? FeatureName { get; set; }

    [CliOption("--feature-store-name")]
    public string? FeatureStoreName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}