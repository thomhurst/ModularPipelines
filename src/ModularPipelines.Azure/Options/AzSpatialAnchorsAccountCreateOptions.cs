using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spatial-anchors-account", "create")]
public record AzSpatialAnchorsAccountCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--storage-account-name")]
    public int? StorageAccountName { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}