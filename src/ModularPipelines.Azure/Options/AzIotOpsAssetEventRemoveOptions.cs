using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "ops", "asset", "event", "remove")]
public record AzIotOpsAssetEventRemoveOptions(
[property: CliOption("--asset")] string Asset,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--en")]
    public string? En { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }
}