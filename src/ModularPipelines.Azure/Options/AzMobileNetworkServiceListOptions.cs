using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mobile-network", "service", "list")]
public record AzMobileNetworkServiceListOptions(
[property: CliOption("--mobile-network-name")] string MobileNetworkName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;