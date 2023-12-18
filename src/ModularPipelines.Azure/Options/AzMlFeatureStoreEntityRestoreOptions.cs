using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "feature-store-entity", "restore")]
public record AzMlFeatureStoreEntityRestoreOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--version")] string Version
) : AzOptions
{
    [CommandSwitch("--feature-store-name")]
    public string? FeatureStoreName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}