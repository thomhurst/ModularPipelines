using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vnet", "subnet", "list-available-delegations")]
public record AzNetworkVnetSubnetListAvailableDelegationsOptions : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}