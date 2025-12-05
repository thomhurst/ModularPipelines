using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vpn-site", "download")]
public record AzNetworkVpnSiteDownloadOptions(
[property: CliOption("--request")] string Request,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vwan-name")] string VwanName
) : AzOptions;