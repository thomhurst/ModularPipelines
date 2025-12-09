using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vpn-site", "link", "list")]
public record AzNetworkVpnSiteLinkListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--site-name")] string SiteName
) : AzOptions;