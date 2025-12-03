using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mobile-network", "site", "list")]
public record AzMobileNetworkSiteListOptions(
[property: CliOption("--mobile-network-name")] string MobileNetworkName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;