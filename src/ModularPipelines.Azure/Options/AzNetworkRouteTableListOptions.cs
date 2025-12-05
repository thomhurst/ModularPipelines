using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "route-table", "list")]
public record AzNetworkRouteTableListOptions : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}