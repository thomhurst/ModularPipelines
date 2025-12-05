using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "feature-store-entity", "create")]
public record AzMlFeatureStoreEntityCreateOptions : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--feature-store-name")]
    public string? FeatureStoreName { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}