using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "feature-store", "list")]
public record AzMlFeatureStoreListOptions : AzOptions
{
    [CommandSwitch("--max-results")]
    public string? MaxResults { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}