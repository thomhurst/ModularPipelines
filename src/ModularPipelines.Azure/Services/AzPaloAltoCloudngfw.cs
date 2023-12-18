using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("palo-alto")]
public class AzPaloAltoCloudngfw
{
    public AzPaloAltoCloudngfw(
        AzPaloAltoCloudngfwFirewall firewall,
        AzPaloAltoCloudngfwLocalRulestack localRulestack
    )
    {
        Firewall = firewall;
        LocalRulestack = localRulestack;
    }

    public AzPaloAltoCloudngfwFirewall Firewall { get; }

    public AzPaloAltoCloudngfwLocalRulestack LocalRulestack { get; }
}