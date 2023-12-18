using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-site", "download")]
public record AzNetworkVpnSiteDownloadOptions(
[property: CommandSwitch("--request")] string Request,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vwan-name")] string VwanName
) : AzOptions
{
}