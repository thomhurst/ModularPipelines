using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns-resolver", "forwarding-rule", "list")]
public record AzDnsResolverForwardingRuleListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--ruleset-name")] string RulesetName
) : AzOptions
{
    [CommandSwitch("--top")]
    public string? Top { get; set; }
}

