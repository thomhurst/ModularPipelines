using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mobile-network", "data-network", "list")]
public record AzMobileNetworkDataNetworkListOptions(
[property: CliOption("--mobile-network-name")] string MobileNetworkName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;