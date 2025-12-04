using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "ops", "asset", "data-point", "list")]
public record AzIotOpsAssetDataPointListOptions(
[property: CliOption("--asset")] string Asset,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;