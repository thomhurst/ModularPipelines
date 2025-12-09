using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ams", "asset-filter", "list")]
public record AzAmsAssetFilterListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--asset-name")] string AssetName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;