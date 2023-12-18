using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "account", "network-profile", "network-rule", "delete")]
public record AzBatchAccountNetworkProfileNetworkRuleDeleteOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ip-address")]
    public string? IpAddress { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--profile")]
    public string? Profile { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}