using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "search-all-iam-policies")]
public record GcloudAssetSearchAllIamPoliciesOptions : GcloudOptions
{
    [CommandSwitch("--asset-types")]
    public string[]? AssetTypes { get; set; }

    [CommandSwitch("--order-by")]
    public string? OrderBy { get; set; }

    [CommandSwitch("--query")]
    public string? Query { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}