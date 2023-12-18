using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "nsg", "rule", "list")]
public record AzNetworkNsgRuleListOptions(
[property: CommandSwitch("--nsg-name")] string NsgName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--include-default")]
    public bool? IncludeDefault { get; set; }
}