using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "asset-track", "list")]
public record AzAmsAssetTrackListOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--asset-name")] string AssetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}