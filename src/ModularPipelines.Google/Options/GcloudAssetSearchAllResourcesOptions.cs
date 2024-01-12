using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "search-all-resources")]
public record GcloudAssetSearchAllResourcesOptions : GcloudOptions
{
    [CommandSwitch("--asset-types")]
    public string[]? AssetTypes { get; set; }

    [CommandSwitch("--order-by")]
    public string? OrderBy { get; set; }

    [CommandSwitch("--query")]
    public string? Query { get; set; }

    [CommandSwitch("--read-mask")]
    public string? ReadMask { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}