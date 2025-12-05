using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "cross-connection", "list")]
public record AzNetworkCrossConnectionListOptions : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}