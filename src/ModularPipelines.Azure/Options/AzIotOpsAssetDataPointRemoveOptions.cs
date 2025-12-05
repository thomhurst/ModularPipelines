using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "ops", "asset", "data-point", "remove")]
public record AzIotOpsAssetDataPointRemoveOptions(
[property: CliOption("--asset")] string Asset,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--data-source")]
    public string? DataSource { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }
}