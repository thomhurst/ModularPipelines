using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "ops", "asset", "data-point", "list")]
public record AzIotOpsAssetDataPointListOptions(
[property: CommandSwitch("--asset")] string Asset,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;