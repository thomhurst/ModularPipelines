using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vnet", "subnet", "list")]
public record AzNetworkVnetSubnetListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vnet-name")] string VnetName
) : AzOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }
}