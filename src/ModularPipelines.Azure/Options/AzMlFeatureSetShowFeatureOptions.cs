using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "feature-set", "show-feature")]
public record AzMlFeatureSetShowFeatureOptions(
[property: CommandSwitch("--feature-name")] string FeatureName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--version")] string Version
) : AzOptions
{
    [CommandSwitch("--feature-store-name")]
    public string? FeatureStoreName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}