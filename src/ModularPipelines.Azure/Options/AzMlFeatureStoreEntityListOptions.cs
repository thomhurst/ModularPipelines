using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "feature-store-entity", "list")]
public record AzMlFeatureStoreEntityListOptions : AzOptions
{
    [BooleanCommandSwitch("--archived-only")]
    public bool? ArchivedOnly { get; set; }

    [CommandSwitch("--feature-store-name")]
    public string? FeatureStoreName { get; set; }

    [BooleanCommandSwitch("--include-archived")]
    public bool? IncludeArchived { get; set; }

    [CommandSwitch("--max-results")]
    public string? MaxResults { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}