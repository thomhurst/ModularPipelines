using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "ops", "asset", "data-point", "add")]
public record AzIotOpsAssetDataPointAddOptions(
[property: CliOption("--asset")] string Asset,
[property: CliOption("--data-source")] string DataSource,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--capability-id")]
    public string? CapabilityId { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--observability-mode")]
    public string? ObservabilityMode { get; set; }

    [CliOption("--qs")]
    public string? Qs { get; set; }

    [CliOption("--sampling-interval")]
    public string? SamplingInterval { get; set; }
}