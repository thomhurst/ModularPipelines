using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "feature-set", "list-materialization-operation")]
public record AzMlFeatureSetListMaterializationOperationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--version")] string Version
) : AzOptions
{
    [CliOption("--feature-store-name")]
    public string? FeatureStoreName { get; set; }

    [CliOption("--feature-window-end-time")]
    public string? FeatureWindowEndTime { get; set; }

    [CliOption("--feature-window-start-time")]
    public string? FeatureWindowStartTime { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}