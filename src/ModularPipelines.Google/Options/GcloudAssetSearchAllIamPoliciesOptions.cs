using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "search-all-iam-policies")]
public record GcloudAssetSearchAllIamPoliciesOptions : GcloudOptions
{
    [CliOption("--asset-types")]
    public string[]? AssetTypes { get; set; }

    [CliOption("--order-by")]
    public string? OrderBy { get; set; }

    [CliOption("--query")]
    public string? Query { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}