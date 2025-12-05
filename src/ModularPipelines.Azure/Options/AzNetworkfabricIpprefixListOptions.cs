using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric", "ipprefix", "list")]
public record AzNetworkfabricIpprefixListOptions : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}