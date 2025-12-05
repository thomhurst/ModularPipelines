using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webpubsub", "create")]
public record AzWebpubsubCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--unit-count")]
    public int? UnitCount { get; set; }
}