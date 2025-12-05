using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "ops", "asset", "event", "list")]
public record AzIotOpsAssetEventListOptions(
[property: CliOption("--asset")] string Asset,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;