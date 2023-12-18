using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "network-rule", "add")]
public record AzAcrNetworkRuleAddOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--ip-address")]
    public string? IpAddress { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}