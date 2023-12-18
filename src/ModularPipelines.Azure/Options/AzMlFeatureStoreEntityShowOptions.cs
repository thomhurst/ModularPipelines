using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "feature-store-entity", "show")]
public record AzMlFeatureStoreEntityShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--version")] string Version
) : AzOptions
{
    [CommandSwitch("--feature-store-name")]
    public string? FeatureStoreName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}