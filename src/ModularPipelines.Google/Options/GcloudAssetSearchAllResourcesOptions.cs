using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "search-all-resources")]
public record GcloudAssetSearchAllResourcesOptions : GcloudOptions
{
    [CliOption("--asset-types")]
    public string[]? AssetTypes { get; set; }

    [CliOption("--order-by")]
    public string? OrderBy { get; set; }

    [CliOption("--query")]
    public string? Query { get; set; }

    [CliOption("--read-mask")]
    public string? ReadMask { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}