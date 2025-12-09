using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mobile-network", "slice", "list")]
public record AzMobileNetworkSliceListOptions(
[property: CliOption("--mobile-network-name")] string MobileNetworkName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;