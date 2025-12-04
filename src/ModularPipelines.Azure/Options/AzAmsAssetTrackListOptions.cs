using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ams", "asset-track", "list")]
public record AzAmsAssetTrackListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--asset-name")] string AssetName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;