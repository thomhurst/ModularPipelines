using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "feature-set", "list")]
public record AzMlFeatureSetListOptions : AzOptions
{
    [CliFlag("--archived-only")]
    public bool? ArchivedOnly { get; set; }

    [CliOption("--feature-store-name")]
    public string? FeatureStoreName { get; set; }

    [CliFlag("--include-archived")]
    public bool? IncludeArchived { get; set; }

    [CliOption("--max-results")]
    public string? MaxResults { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}