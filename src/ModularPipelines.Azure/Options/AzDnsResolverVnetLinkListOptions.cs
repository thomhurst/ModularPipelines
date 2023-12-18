using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns-resolver", "vnet-link", "list")]
public record AzDnsResolverVnetLinkListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--ruleset-name")] string RulesetName
) : AzOptions
{
    [CommandSwitch("--top")]
    public string? Top { get; set; }
}