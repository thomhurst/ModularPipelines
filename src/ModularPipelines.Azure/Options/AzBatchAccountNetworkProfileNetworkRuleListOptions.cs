using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "account", "network-profile", "network-rule", "list")]
public record AzBatchAccountNetworkProfileNetworkRuleListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }
}