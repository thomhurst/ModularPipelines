using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "nsg", "rule", "list")]
public record AzNetworkNsgRuleListOptions(
[property: CliOption("--nsg-name")] string NsgName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--include-default")]
    public bool? IncludeDefault { get; set; }
}