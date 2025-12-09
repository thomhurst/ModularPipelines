using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "feature-set", "archive")]
public record AzMlFeatureSetArchiveOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--version")] string Version
) : AzOptions
{
    [CliOption("--feature-store-name")]
    public string? FeatureStoreName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}