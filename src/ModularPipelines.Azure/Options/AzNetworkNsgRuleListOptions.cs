using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

